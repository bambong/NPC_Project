using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using static AmazingAssets.WireframeShader.WireframeMaskController;

[RequireComponent(typeof(Collider))]
public class DebugZone : MonoBehaviour
{
    [SerializeField]
    private int playerSlotCount =2;

    [SerializeField]
    private List<KeywordEntity> childEntitys = new List<KeywordEntity>(); 

    [SerializeField]
    private GameObject[] keywords;

    [Header("Wrie Material list")]
    [SerializeField]
    private Material[] wireMaterials;

    public int PlayerSlotCount { get => playerSlotCount; }
    public Material[] WireMaterials { get => wireMaterials; }

    private List<PlayerKeywordFrame> playerFrames = new List<PlayerKeywordFrame>();
    private Transform playerLayout;

    private Vector3 boxSize;
    private void Start()
    {
        MakeFrame();
        MakeKeyword();
        WireMaterialClear();
        for (int i = 0; i< childEntitys.Count; ++i) 
        {
            childEntitys[i].SetDebugZone(this);
        }
        boxSize = GetComponent<BoxCollider>().bounds.size;
    }
    private void MakeFrame() 
    {
        playerLayout = Managers.Resource.Instantiate("Layout",Managers.Keyword.PlayerPanelLayout).transform;
        ClosePlayerLayout();
        for(int i = 0; i < playerSlotCount; ++i)
        {
            playerFrames.Add(Managers.UI.MakeSubItem<PlayerKeywordFrame>(playerLayout,"KeywordPlayerSlotUI"));
        }
    }
    private void MakeKeyword() 
    { 
        for(int i = 0; i< keywords.Length; ++i) 
        {
            var keyword =Managers.UI.MakeSubItem<KeywordController>(null,"KeywordPrefabs/" + keywords[i].name);

            if(!RegisterKeyword(keyword)) 
            {
                Debug.LogError($" DebugZone : {gameObject.name} 슬롯 갯수 이상의 키워드 생성");
                return;
            }
        }
    }
    private bool RegisterKeyword(KeywordController keyword)
    {
        for(int i =0; i < playerFrames.Count; ++i) 
        {
            if(playerFrames[i].HasKeyword) 
            {
                continue;
            }
            playerFrames[i].InitKeyword(keyword);
            keyword.SetDebugZone(this);
            return true;
        }

        return false;
    }
    private void WireMaterialClear() 
    {
        for (int i = 0; i < WireMaterials.Length; i++)
        {
            if (WireMaterials[i] == null)
                continue;

            WireMaterials[i].SetVector("_WireframeShaderMaskSpherePosition",Vector3.zero);

            WireMaterials[i].SetFloat("_WireframeShaderMaskSphereRadius", 0);
            WireMaterials[i].SetVector("_WireframeShaderMaskBoxBoundingBox", Vector3.zero);
        }
    }
    public void OpenPlayerLayout() => playerLayout.gameObject.SetActive(true);
    public void ClosePlayerLayout() => playerLayout.gameObject.SetActive(false);
    public void OnEnterDebugMod() 
    {
        Managers.Game.Player.SetWireframeMaterial(wireMaterials);
        Managers.Game.Player.OpenWireEffect(boxSize *2);
    }
    public void OnExitDebugMod() 
    {
        Managers.Game.Player.CloseWireEffect();
        ClosePlayerLayout();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Keyword.SetDebugZone(this);
            Managers.Game.Player.isDebugButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Keyword.SetDebugZone(null);
            Managers.Game.Player.isDebugButton();
        }
    }
}
