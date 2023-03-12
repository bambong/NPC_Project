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

    private Vector3 prevGravity;
    private float DEBUG_TIME_SCALE = 0.2f;
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
        playerKeywordPanel.Open();
        Managers.Time.SetTimeScale(TIME_TYPE.NONE_PLAYER, DEBUG_TIME_SCALE);
        prevGravity = Physics.gravity;
        Physics.gravity = prevGravity * DEBUG_TIME_SCALE;
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
        playerKeywordPanel.Close();
        Physics.gravity = prevGravity; 
        Managers.Time.SetTimeScale(TIME_TYPE.NONE_PLAYER,1);
        Managers.Game.SetStateNormal();
    }
    public void SetDebugZone(DebugZone zone)
    {
        curDebugZone = zone;
    }
    public void EnterKeywordMod(KeywordEntity keywordEntity) 
    {
        if (!keywordEntity.IsAvailable || curKeywordEntity == keywordEntity) 
        {
            return;
        }
        if(curKeywordEntity != null) 
        {
            curKeywordEntity.CloseKeywordSlot();
        }
        //keywordEntity.OpenKeywordSlot();
        //Managers.Game.SetStateKeywordMod();
        curKeywordEntity = keywordEntity;
        curKeywordEntity.OpenKeywordSlot();
    }
  
    public void ExitKeywordMod()
    {
        if(curKeywordEntity != null) 
        {
            curKeywordEntity.CloseKeywordSlot();
            curKeywordEntity = null;
        }
       // Managers.Game.SetStateDebugMod();
        // curKeywordEntity.DecisionKeyword();
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
