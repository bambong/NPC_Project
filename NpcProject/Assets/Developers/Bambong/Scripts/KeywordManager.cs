using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordManager
{

    private KeywordEntity curKeywordEntity;
    private PlayerKeywordPanelController playerKeywordPanel;
    public Transform playerKeywordPanelLayout { get => playerKeywordPanel.Layout.transform; }
    public PlayerKeywordPanelController PlayerKeywordPanel { get => playerKeywordPanel;}
    public KeywordEntity CurKeywordEntity { get => curKeywordEntity; }

    private List<KeywordController> curPlayerKeywords = new List<KeywordController>();
    private List<KeywordEntity> curSceneEntity = new List<KeywordEntity>();
    private GraphicRaycaster graphicRaycaster;
    public void Init()
    {
        playerKeywordPanel = Managers.UI.MakeSceneUI<PlayerKeywordPanelController>(null,"PlayerKeywordPanel");
        graphicRaycaster = playerKeywordPanel.gameObject.GetOrAddComponent<GraphicRaycaster>();
    }

    public List<RaycastResult> GetRaycastList(PointerEventData pointerEventData)
    {
        var raycastResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData,raycastResults);
        return raycastResults;
    }
    public void AddSceneEntity(KeywordEntity entity) => curSceneEntity.Add(entity);

    public void EnterDebugMod() 
    {
        foreach(var entity in curSceneEntity)
        {
            entity.OpenWorldSlotUI();
        }
    }
    public void ExitDebugMod()
    {
        foreach(var entity in curSceneEntity)
        {
            entity.CloseWorldSlotUI();
        }
    }
    public void EnterKeywordMod(KeywordEntity keywordEntity) 
    {
        Managers.Game.SetStateKeywordMod();
        curKeywordEntity = keywordEntity;
        playerKeywordPanel.Open();
        UpdateKeywordLayout();
        keywordEntity.OpenKeywordSlot();
    }
  
    public void ExitKeywordMod()
    {
        playerKeywordPanel.Layout.enabled = true;
        Managers.Game.SetStateDebugMod();
        playerKeywordPanel.Close();
        curKeywordEntity.CloseKeywordSlot();
        curKeywordEntity.DecisionKeyword();
    }
    public bool AddKeywordToPlayer(KeywordController keywordController) 
    {
        if(curPlayerKeywords.Contains(keywordController)) 
        {
            return false;
        }
        UpdateKeywordLayout();
        curPlayerKeywords.Add(keywordController);
        playerKeywordPanel.AddKeyword(keywordController);
        return true;
    }
    public bool RemoveKeywordToPlayer(KeywordController keywordController) 
    {
        if(!curPlayerKeywords.Contains(keywordController))
        {
            return false;
        }
        curPlayerKeywords.Remove(keywordController);
        return true;
    }
    public void SetKeyWord(KeywordController keywordController)
    {
        keywordController.ResetKeyword();
    }
    public void UpdateKeywordLayout()
    {
        Managers.Scene.CurrentScene.StartCoroutine(UpdateKeywordLayoutco());
    }
    public void Clear()
    {
        curPlayerKeywords.Clear();
        curSceneEntity.Clear();
    }
    IEnumerator UpdateKeywordLayoutco()
    {
        playerKeywordPanel.Layout.enabled = true;
        yield return null;
        playerKeywordPanel.Layout.enabled = false;
    }

}
