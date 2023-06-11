
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEventTrigger : EventTrigger
{
    [SerializeField]
    private Transform lightTarget;

    [SerializeField]
    private bool shadowColorTrans;
    [SerializeField]
    private Color color;

    public override void OnEventTrigger(Collider other)
    {
        if (shadowColorTrans) 
        {
            Managers.Game.Player.ShadowController.SetColor(color);
        }
        Managers.Game.Player.ShadowController.SetLight(lightTarget);
    }
}
