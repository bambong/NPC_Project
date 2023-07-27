using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActionShooting : LaserAction
{
    [SerializeField]
    private NPCLaser npcLaser;

    public override void StartLaserEvent()
    {
        npcLaser.StartLaser();
    }

    public override void StopLaserEvent()
    {
        npcLaser.StopLaser();
    }
}
