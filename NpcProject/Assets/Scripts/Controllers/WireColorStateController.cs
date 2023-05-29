using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class WireColorStateController 
{
    public enum E_WIRE_STATE 
    {
        NORMAL,
        PAIR,
    }

    private bool isWireStateOn = false;
    private bool isMoveAble = true;
    private KeywordEntity entity;
    private Dictionary<E_WIRE_STATE, List<E_WIRE_COLOR_MODE>> colorMods;

    public void Init(KeywordEntity entity) 
    {
        this.entity = entity;
        colorMods = new Dictionary<E_WIRE_STATE, List<E_WIRE_COLOR_MODE>>();
        colorMods.Add(E_WIRE_STATE.NORMAL, new List<E_WIRE_COLOR_MODE>());
        colorMods.Add(E_WIRE_STATE.PAIR, new List<E_WIRE_COLOR_MODE>());
    }
    public void MoveAbleUpdate(bool isOn) 
    {

        if(isMoveAble == isOn) 
        {
            return;
        }
        isMoveAble = isOn;
        if (!isMoveAble) 
        {
            Color pickColor = Managers.Keyword.GetColorByState(E_WIRE_COLOR_MODE.Fail);
            entity.SetWireFrameColor(pickColor);
            //Managers.Sound.PlaySFX(Define.SOUND.ErrorEffectKeyword);
        }
        else 
        {
            UpdateColor();
        }
    }
    public void Open() 
    {
        if (isWireStateOn) 
        {
            return;
        }
        isWireStateOn = true;
        UpdateColor();
    }
    public void Close()
    {
        if (!isWireStateOn)
        {
            return;
        }
        isWireStateOn = false;

        //entity.ClearWireFrameColor();
        UpdateColor();    
    }

    public void AddColorState(E_WIRE_STATE wireState, E_WIRE_COLOR_MODE mod) 
    {
        colorMods[wireState].Add(mod);
        UpdateColor();
    }
    public void RemoveColorState(E_WIRE_STATE wireState, E_WIRE_COLOR_MODE mod) 
    {
        colorMods[wireState].Remove(mod);
        UpdateColor();
    }
    public void UpdateColor() 
    {

        if(!isMoveAble)
        {
            return;
        }
        E_WIRE_STATE wireMod = E_WIRE_STATE.NORMAL;
        Color pickColor = Managers.Keyword.GetColorByState(E_WIRE_COLOR_MODE.Default);
        if (isWireStateOn) 
        {
            wireMod = E_WIRE_STATE.PAIR;
            pickColor = Managers.Keyword.GetColorByState(E_WIRE_COLOR_MODE.Pair_Default);
        }
        
        if(colorMods[wireMod].Count > 0) 
        {
            pickColor = Managers.Keyword.GetColorByState(colorMods[wireMod][colorMods[wireMod].Count - 1]);
        }
        entity.SetWireFrameColor(pickColor);
    }
}
