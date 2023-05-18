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
    public virtual void Stop()
    {
    }
    public virtual void Play()
    {
    }

    public virtual void SetInput(Vector3 moveVec) 
    {
    
    }
}
