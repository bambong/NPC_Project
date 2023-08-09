using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserActionEvent : LaserAction
{
    [SerializeField]
    private UnityEvent laserEvent;

    public override void StartLaserEvent()
    {
        laserEvent.Invoke();
    }

    public override void StopLaserEvent()
    {
    }
}
