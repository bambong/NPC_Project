using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordEvent : GameEvent
{
    public override void Play()
    {
        onStart?.Invoke();
    }
    
    

}



public class KeywordManager
{

    private KeywordEntity curKeywordEntity;
    private PlayerKeywordPanelController playerKeywordPanel;
    public Transform playerKeywordPanelLayout { get => playerKeywordPanel.Layout.transform; }
    public PlayerKeywordPanelController PlayerKeywordPanel { get => playerKeywordPanel;}
    public KeywordEntity CurKeywordEntity { get => curKeywordEntity; }
    public bool IsDebugZoneIn { get => curDebugZone != null; }

    private List<KeywordController> curPlayerKeywords = new List<KeywordController>();
    private List<KeywordEntity> curSceneEntity = new List<KeywordEntity>();
    private List<DebugModEffectController> debugModEffectControllers = new List<DebugModEffectController>();
    private GraphicRaycaster graphicRaycaster;

    private DebugZone curDebugZone = null;


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
        curDebugZone.OnEnterDebugMod();
        foreach(var entity in curSceneEntity)
        {
            entity.EnterDebugMod();
        }
        foreach(var effect in debugModEffectControllers) 
        {
            effect.EnterDebugMod();
        }
    }
    
    public void ExitDebugMod()
    {
        curDebugZone.OnExitDebugMod();
        foreach(var entity in curSceneEntity)
        {
            entity.ExitDebugMod();
        }
        foreach (var effect in debugModEffectControllers)
        {
            effect.ExitDebugMod();
        }
    }
    public void SetDebugZone(DebugZone zone)
    {
        curDebugZone = zone;
    }
    public void EnterKeywordMod(KeywordEntity keywordEntity) 
    {
        keywordEntity.OpenKeywordSlot();
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

    public void AddDebugEffectController(DebugModEffectController debugModEffectController) 
    {
        debugModEffectControllers.Add(debugModEffectController);
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
        debugModEffectControllers.Clear();
        curPlayerKeywords.Clear();
        curSceneEntity.Clear();
        curDebugZone = null;
    }
    IEnumerator UpdateKeywordLayoutco()
    {
        playerKeywordPanel.Layout.enabled = true;
        yield return null;
        playerKeywordPanel.Layout.enabled = false;
    }

}
