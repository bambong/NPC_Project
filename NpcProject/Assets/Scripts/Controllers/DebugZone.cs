using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
//using DG.Tweening;
//using static AmazingAssets.WireframeShader.WireframeMaskController;
//using Unity.Services.Analytics.Platform;
//using UnityEditorInternal;

[RequireComponent(typeof(Collider))]
public class DebugZone : GuIdBehaviour, IDataHandler
{
    [Header("플레이어 초기화 기본 위치")]
    [SerializeField]
    private Vector3 playerDefaultPos;

    [Space(10)]
    [Header("디버그 모드 제한 시간 -1  = Infinity")]
    [SerializeField]
    private float debugAbleTime = -1f;
    [SerializeField]
    private float coolTime = 1f;

    [SerializeField]
    private int playerSlotCount =2;

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
    public Vector3 PlayerDefaultPos { get => playerDefaultPos; }

    protected override void Start()
    {
        base.Start();
        MakeFrame();
        InitKeywords();
        MakeDebugGaugeUi();
        WireMaterialClear();
        boxSize = GetComponent<BoxCollider>().bounds.size;
        if(playerDefaultPos == Vector3.zero) 
        {
            playerDefaultPos = Managers.Scene.CurrentScene.PlayerSpawnSpot;
        }
    }
    private void MakeFrame() 
    {
        playerLayout = Managers.Resource.Instantiate("PlayerLayout", Managers.Keyword.PlayerPanelLayout).transform;
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
            keyword.isPlay = true;
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

        var length = boxSize.magnitude;
        Managers.Game.Player.OpenWireEffect(new Vector3(length,length,length));
        
        StartCoroutine(DebugModeTimeUpdate());
       
    }
    public void OnExitDebugMod() 
    {
        Managers.Game.Player.CloseWireEffect();
        
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
            Managers.Data.SaveGame();
            OpenPlayerLayout();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Keyword.SetDebugZone(null);
            Managers.Game.Player.isDebugButton();
            Managers.Game.RetryPanel.CloseResetButton();
            ClosePlayerLayout();
        }
    }

    public void LoadData(GameData gameData)
    {
        if(!gameData.debugZoneDatas.ContainsKey(GuId)) 
        {
            return;
        }
        var frameDatas = gameData.debugZoneDatas[GuId].playerFramDatas;
        List<GameObject> temp = new List<GameObject>();
        for(int i =0; i < frameDatas.Count; ++i) 
        {
            //if (frameDatas[i] == "") 
            //{
            //    temp.Add(null);
            //    continue;
            //}

            temp.Add(Managers.Resource.Load<GameObject>($"Prefabs/UI/SubItem/KeywordPrefabs/{frameDatas[i]}"));
        }
        keywords = temp.ToArray();
    }
    

    public void SaveData(GameData gameData)
    {
        var data = new DebugZoneData();

        for (int i = 0; i < playerFrames.Count; ++i)
        {
            if (playerFrames[i].HasKeyword)
            {
                data.playerFramDatas.Add(playerFrames[i].CurFrameInnerKeyword.KewordId);
                continue;
            }
           // data.playerFramDatas.Add("");
        }
        gameData.debugZoneDatas.AddOrUpdateValue(guId, data);
    }
}
