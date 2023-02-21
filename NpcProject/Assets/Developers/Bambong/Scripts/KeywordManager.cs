using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Scripting;
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
    public PlayerKeywordPanelController PlayerKeywordPanel { get => playerKeywordPanel;}
    public KeywordEntity CurKeywordEntity { get => curKeywordEntity; }
    public bool IsDebugZoneIn { get => curDebugZone != null; }
    public DebugZone CurDebugZone { get => curDebugZone;  }

    private List<KeywordEntity> curSceneEntity = new List<KeywordEntity>();
    private List<DebugModEffectController> debugModEffectControllers = new List<DebugModEffectController>();
    private GraphicRaycaster graphicRaycaster;

    private DebugZone curDebugZone = null;
    public Transform PlayerPanelLayout { get => playerKeywordPanel.LayoutParent; }

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
        Managers.Game.SetStateNormal();
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
        keywordEntity.OpenKeywordSlot();
    }
  
    public void ExitKeywordMod()
    {
        Managers.Game.SetStateDebugMod();
        playerKeywordPanel.Close();
        curKeywordEntity.CloseKeywordSlot();
        curKeywordEntity.DecisionKeyword();
    }

    public void AddDebugEffectController(DebugModEffectController debugModEffectController) 
    {
        debugModEffectControllers.Add(debugModEffectController);
    }

    public void Clear()
    {
        debugModEffectControllers.Clear();
        curSceneEntity.Clear();
        curDebugZone = null;
    }

}
