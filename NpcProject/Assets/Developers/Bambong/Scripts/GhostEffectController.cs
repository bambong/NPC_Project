using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostEffectController : MonoBehaviour
{

    protected GhostEffectColorPickerController colorPicker;
    public void Init(GhostEffectColorPickerController colorPicker) 
    {
        this.colorPicker = colorPicker;
    }
    public abstract void Open();

    public abstract void Close();

    
}
