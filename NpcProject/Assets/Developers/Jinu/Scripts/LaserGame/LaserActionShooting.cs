using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActionShooting : LaserAction
{
    [SerializeField]
    private NPCLaser npcLaser;

    public override void StartLaserEvent()
    {
        npcLaser.gameObject.SetActive(true);
    }

    public override void StopLaserEvent()
    {
        npcLaser.OnStop();
        npcLaser.gameObject.SetActive(false);
    }
}
