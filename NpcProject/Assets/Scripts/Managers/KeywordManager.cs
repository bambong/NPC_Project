using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum E_WIRE_COLOR_MODE
{
    Default = 0,
    Pair_Default ,
    Apart,
    Attach,
    Float, 
    Revolution = 5,
    
}

public class KeywordManager
{
    private PlayerKeywordPanelController playerKeywordPanel;

    private List<KeywordEntity> curSceneEntity = new List<KeywordEntity>();
    private List<DebugModEffectController> debugModEffectControllers = new List<DebugModEffectController>();
    private GraphicRaycaster graphicRaycaster;

    private DebugZone curDebugZone = null;
    private Dictionary<E_WIRE_COLOR_MODE, Color> colorStateDic = new Dictionary<E_WIRE_COLOR_MODE, Color>();
    public PlayerKeywordPanelController PlayerKeywordPanel { get => playerKeywordPanel;}
    public bool IsDebugZoneIn { get => curDebugZone != null && curDebugZone.IsDebugAble; }
    public DebugZone CurDebugZone { get => curDebugZone;  }

    public Transform PlayerPanelLayout { get => playerKeywordPanel.LayoutParent; }
    public Transform KeywordEntitySlots { get; private set; } 
    public KeywordController CurDragKeyword { get; set; }
    public Action OnEnterDebugModEvent{ get; set; }
    public Action OnExitDebugModEvent{ get; set; }
    public void Init()
    {
        playerKeywordPanel = Managers.UI.MakeSceneUI<PlayerKeywordPanelController>(null,"PlayerKeywordPanel");
        KeywordEntitySlots = new GameObject("KeywordEntitySlots").transform;
        KeywordEntitySlots.SetParent(playerKeywordPanel.transform);
        graphicRaycaster = playerKeywordPanel.gameObject.GetOrAddComponent<GraphicRaycaster>();
        LoadColorStateData();
    }
    public void LoadColorStateData() 
    {
        var colorData = Resources.Load<ColorStateData>("Data/ColorStateData");

        foreach(var item in colorData.colorStates) 
        {
            colorStateDic.Add(item.mode,item.color);
        }
    }
    public void OnSceneLoaded() 
    {
        curSceneEntity.Clear();
        Init();
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
    public Color GetColorByState(E_WIRE_COLOR_MODE mod) 
    {
        return colorStateDic[mod];
    }
    public void AddSceneEntity(KeywordEntity entity) => curSceneEntity.Add(entity);
    public void RemoveSceneEntity(KeywordEntity entity) => curSceneEntity.Remove(entity);
    public void EnterDebugMod() 
    {
       
        foreach (var entity in curSceneEntity)
        {
            entity.EnterDebugMod();
        }
        foreach(var effect in debugModEffectControllers) 
        {
            effect.EnterDebugMod();
        }
        curDebugZone.OnEnterDebugMod();
        playerKeywordPanel.Open();
       
        OnEnterDebugModEvent?.Invoke();
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
       // playerKeywordPanel.Close();
        Managers.Game.SetStateNormal();

        OnExitDebugModEvent?.Invoke();
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
        colorStateDic.Clear();
        curDebugZone = null;
        OnExitDebugModEvent = null;
        OnEnterDebugModEvent = null;

    }

}
