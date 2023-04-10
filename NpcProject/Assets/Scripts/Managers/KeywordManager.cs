using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Scripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordManager
{
    private KeywordEntity curKeywordEntity;

    private PlayerKeywordPanelController playerKeywordPanel;
    public PlayerKeywordPanelController PlayerKeywordPanel { get => playerKeywordPanel;}
    public KeywordEntity CurKeywordEntity { get => curKeywordEntity; }
    public bool IsDebugZoneIn { get => curDebugZone != null && curDebugZone.IsDebugAble; }
    public DebugZone CurDebugZone { get => curDebugZone;  }

    private List<KeywordEntity> curSceneEntity = new List<KeywordEntity>();
    private List<DebugModEffectController> debugModEffectControllers = new List<DebugModEffectController>();
    private GraphicRaycaster graphicRaycaster;

    private DebugZone curDebugZone = null;
    public Transform PlayerPanelLayout { get => playerKeywordPanel.LayoutParent; }
    public Transform KeywordEntitySlots { get; private set; } 
    public KeywordController CurDragKeyword { get; set; }
    public void Init()
    {
        playerKeywordPanel = Managers.UI.MakeSceneUI<PlayerKeywordPanelController>(null,"PlayerKeywordPanel");
        KeywordEntitySlots = new GameObject("KeywordEntitySlots").transform;
        KeywordEntitySlots.SetParent(playerKeywordPanel.transform);
        graphicRaycaster = playerKeywordPanel.gameObject.GetOrAddComponent<GraphicRaycaster>();
    }
    public void OnSceneLoaded() 
    {
        Init();
    }
    public void OnSceneLoadComplete() 
    {
    }
    public void AddKeywordMakerGauge(int amount)
    {
        playerKeywordPanel.AddKeywordMakerGauge(amount);
    }
    public List<RaycastResult> GetRaycastList(PointerEventData pointerEventData)
    {
        var raycastResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData,raycastResults);
        return raycastResults;
    }
    public void AddSceneEntity(KeywordEntity entity) => curSceneEntity.Add(entity);
    public void RemoveSceneEntity(KeywordEntity entity) => curSceneEntity.Remove(entity);
    public void EnterDebugMod() 
    {        
        foreach(var entity in curSceneEntity)
        {
            entity.EnterDebugMod();
        }
        foreach(var effect in debugModEffectControllers) 
        {
            effect.EnterDebugMod();
        }
        curDebugZone.OnEnterDebugMod();
        playerKeywordPanel.Open();
    }
    
    public void ExitDebugMod()
    {
        foreach(var entity in curSceneEntity)
        {
            entity.ExitDebugMod();
        }
        foreach (var effect in debugModEffectControllers)
        {
            effect.ExitDebugMod();
        }
        if(CurDragKeyword != null) 
        {
            CurDragKeyword.DragReset();
            CurDragKeyword = null;
        }
        curDebugZone?.OnExitDebugMod();
        playerKeywordPanel.Close();
        Managers.Game.SetStateNormal();
    }
    public void SetDebugZone(DebugZone zone)
    {
        if (zone == null && Managers.Game.IsDebugMod)
        {
            curDebugZone?.OnExitDebugMod();
            Managers.Game.Player.ExitDebugMod();
        }
        curDebugZone = zone;
    }

    public KeywordController MakeKeywordToCurPlayerPanel(string name) 
    {
        return curDebugZone.MakeKeyword(name);
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
