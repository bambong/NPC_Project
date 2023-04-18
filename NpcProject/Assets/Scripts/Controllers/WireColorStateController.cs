using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireColorStateController 
{
    private bool isWireStateOn = false;

    private KeywordEntity entity;
    private List<E_PAIRCOLOR_MODE> colorMods;
    public void Init(KeywordEntity entity) 
    {
        this.entity = entity;
        colorMods = new List<E_PAIRCOLOR_MODE>();
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

        entity.ClearWireFrameColor();
    }

    public void AddColorState(E_PAIRCOLOR_MODE mod) 
    {
        colorMods.Add(mod);
        if (isWireStateOn) 
        {
            UpdateColor();
        }
    }
    public void RemoveColorState(E_PAIRCOLOR_MODE mod) 
    {
        colorMods.Remove(mod);
        if (isWireStateOn)
        {
            UpdateColor();
        }
    }
    public void UpdateColor() 
    {
        var pickColor = Managers.Keyword.GetColorByState(E_PAIRCOLOR_MODE.Default);
        if(colorMods.Count > 0) 
        {
            pickColor = Managers.Keyword.GetColorByState(colorMods[colorMods.Count - 1]);
        }
        entity.SetWireFrameColor(pickColor);
    }

   

}
