using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActionDestroy : LaserAction
{
    [SerializeField]
    private GameObject destroyObj;

    public override void StartLaserEvent()
    {
        destroyObj.SetActive(false);
        Managers.Effect.PlayEffect(Define.EFFECT.MonsterDeathEffect, this.transform);
        Managers.Sound.PlaySFX(Define.SOUND.DeathPlayer);
        Destroy(this);
    }

    public override void StopLaserEvent()
    {
        
    }
}
