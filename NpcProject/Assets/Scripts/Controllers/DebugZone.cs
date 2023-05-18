using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
//using DG.Tweening;
//using static AmazingAssets.WireframeShader.WireframeMaskController;
//using Unity.Services.Analytics.Platform;
//using UnityEditorInternal;

[RequireComponent(typeof(Collider))]
public class DebugZone : MonoBehaviour
{
    [Header("디버그 모드 제한 시간 -1  = Infinity")]
    [SerializeField]
    private float debugAbleTime = -1f;
    [SerializeField]
    private float coolTime = 1f;

    [SerializeField]
    private int playerSlotCount =2;

    [SerializeField]
    private List<KeywordEntity> childEntitys = new List<KeywordEntity>(); 

    [SerializeField]
    private GameObject[] keywords;

    [Header("Wrie Material list")]
    [SerializeField]
    private List<Material> wireMaterials;

    private DebugGaugeUiController debugGaugeUi;

    private List<PlayerKeywordFrame> playerFrames = new List<PlayerKeywordFrame>();
    private Transform playerLayout;
    private Vector3 boxSize;
    private bool isDebugAble = true;
    public int PlayerSlotCount { get => playerSlotCount; }
    public List<Material> WireMaterials { get => wireMaterials; }
    public bool IsDebugAble { get => isDebugAble; }
    private void Start()
    {
        MakeFrame();
        InitKeywords();
       
        MakeDebugGaugeUi();
        
        for (int i = 0; i< childEntitys.Count; ++i) 
        {
            childEntitys[i].SetDebugZone(this);
        }
        WireMaterialClear();
        boxSize = GetComponent<BoxCollider>().bounds.size;
    }
    private void MakeFrame() 
    {
        playerLayout = Managers.Resource.Instantiate("Layout",Managers.Keyword.PlayerPanelLayout).transform;
        ClosePlayerLayout();
        for(int i = 0; i < playerSlotCount; ++i)
        {
            var frame = Managers.UI.MakeSubItem<PlayerKeywordFrame>(playerLayout, "KeywordPlayerSlotUI");
            frame.SetKeywordType(E_KEYWORD_TYPE.ALL);
            playerFrames.Add(frame);
        }
    }
    private void InitKeywords() 
    { 
        for(int i = 0; i< keywords.Length; ++i) 
        {
            if(MakeKeyword(keywords[i].name) == null) 
            {
                Debug.LogError($" DebugZone : {gameObject.name} 슬롯 갯수 이상의 키워드 생성");
                return;
            }
        }
    }
    public KeywordController MakeKeyword(string name)
    {
        KeywordController keyword;

        for (int i = 0; i < playerFrames.Count; ++i)
        {
            if (playerFrames[i].HasKeyword)
            {
                continue;
            }
            keyword = Managers.UI.MakeSubItem<KeywordController>(null, "KeywordPrefabs/" + name);
            playerFrames[i].InitKeyword(keyword);
            keyword.SetDebugZone(this);
            return keyword;
        }
        return null;
    }
    public void MakeDebugGaugeUi() 
    {
        debugGaugeUi = Managers.UI.MakeSubItem<DebugGaugeUiController>(Managers.Keyword.PlayerKeywordPanel.DebugGaugePanel.transform, "DebugGaugeUI");
        debugGaugeUi.transform.localPosition = Vector3.zero;
        debugGaugeUi.Close();
    }

    private void WireMaterialClear() 
    {
        for (int i = 0; i < WireMaterials.Count; i++)
        {
            if (WireMaterials[i] == null)
                continue;

            WireMaterials[i].SetVector("_WireframeShaderMaskSpherePosition",Vector3.zero);
            WireMaterials[i].SetFloat("_WireframeShaderMaskSphereRadius", 0);
            WireMaterials[i].SetVector("_WireframeShaderMaskBoxBoundingBox", Vector3.zero);
        }
    }
    public void AddWireFrameMat(Material mat) 
    {
        if (wireMaterials.Contains(mat) || (!mat.HasProperty("_Wireframe_Color"))) 
        {
            return;
        }
        wireMaterials.Add(mat);

        if(Managers.Keyword.CurDebugZone == this) 
        {
            Managers.Game.Player.AddWireframeMaterial(mat);
        }

    }

    public void OpenPlayerLayout() => playerLayout.gameObject.SetActive(true);
    public void ClosePlayerLayout() => playerLayout.gameObject.SetActive(false);
    public void OnEnterDebugMod() 
    {
        Managers.Game.Player.SetWireframeMaterial(wireMaterials);
        Managers.Game.Player.OpenWireEffect(boxSize *2);
        
        StartCoroutine(DebugModeTimeUpdate());
       
    }
    public void OnExitDebugMod() 
    {
        Managers.Game.Player.CloseWireEffect();
        ClosePlayerLayout();
        debugGaugeUi.Close();
        StartCoroutine(NormalModeTimeUpdate());
    }

    IEnumerator DebugModeTimeUpdate() 
    {
        debugGaugeUi.Open(debugAbleTime);
        if (debugAbleTime < 0)
        {
            yield break;
        }
        float curTime = 0; 
        while (Managers.Game.IsDebugMod) 
        {
            curTime += Managers.Time.GetDeltaTime(TIME_TYPE.PLAYER);
            debugGaugeUi.GaugeUiUpdate(1f-(curTime/debugAbleTime));
            if (curTime >= debugAbleTime) 
            {
                Managers.Game.Player.ExitDebugMod();
                break;
            }
            yield return null;
        }
    }
    IEnumerator NormalModeTimeUpdate() 
    {
        isDebugAble = false;
        float curTime = 0;
        while (curTime < coolTime)
        {
            curTime += Managers.Time.GetDeltaTime(TIME_TYPE.PLAYER);
            
            yield return null;
        }
        isDebugAble = true;
        Managers.Game.Player.isDebugButton();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Keyword.SetDebugZone(this);
            Managers.Game.Player.isDebugButton();
            Managers.Game.RetryPanel.OpenResetButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Keyword.SetDebugZone(null);
            Managers.Game.Player.isDebugButton();
            Managers.Game.RetryPanel.CloseResetButton();
        }
    }
    
}
