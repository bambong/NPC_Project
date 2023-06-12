
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEventTrigger : EventTrigger
{

    [SerializeField]
    private Transform lightTarget;

    [SerializeField]
    private bool immediatelySet;
    [SerializeField]
    private Vector3 immediateRotate;

    [SerializeField]
    private bool shadowColorTrans;
    [SerializeField]
    private Color color;

    [SerializeField]
    private int mipLevel = 0;
    public override void OnEventTrigger(Collider other)
    {
        if (mipLevel > 0) 
        {
            Managers.Game.Player.ShadowController.SetMipLevel(mipLevel);
        }
        if (shadowColorTrans) 
        {
            Managers.Game.Player.ShadowController.SetColor(color);
        }
        if (immediatelySet) 
        {
            Managers.Game.Player.ShadowController.SetLightImmediately(immediateRotate);
        }
        else 
        {
            Managers.Game.Player.ShadowController.SetLightTarget(lightTarget);
        }
    }
}
