using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerrackController : KeywordEntity
{
    [SerializeField]
    private OutlineEffect outlineEffect;


    public override void EnterDebugMod()
    {
        base.EnterDebugMod();
        outlineEffect.OutLineGo.SetActive(true);
    }
    public override void ExitDebugMod()
    {
        base.ExitDebugMod();
        outlineEffect.OutLineGo.SetActive(false);
    }
}
