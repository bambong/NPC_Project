using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModEffectController : MonoBehaviour
{
    public virtual void EnterDebugMod() { } 
    public virtual void ExitDebugMod() { }

    private void Start()
    {
        Init();
    }
    public virtual void Init() 
    {
        Managers.Keyword.AddDebugEffectController(this);
    }
}
