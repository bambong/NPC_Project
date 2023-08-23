using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActionDamage : LaserAction
{
    [SerializeField]
    private int damage = 2;

    public override void StartLaserEvent()
    {
        Managers.Game.Player.GetDamage(damage);
        StartCoroutine(RepeatDamage());
    }

    public override void StopLaserEvent()
    {
        second = 0;
    }

    IEnumerator RepeatDamage()
    {
        second = 0;
        while (second < waitTime && hitcount >= 1)
        {
            second += Time.deltaTime;
            yield return null;
        }
        if(hitcount >= 1)
        {
            StartLaserEvent();
        }
    }
}
