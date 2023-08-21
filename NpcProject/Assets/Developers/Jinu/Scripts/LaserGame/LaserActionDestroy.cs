using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActionDestroy : LaserAction
{
    [SerializeField]
    private KeywordEntity keyword;
   
    public override void StartLaserEvent()
    {
        keyword.IsDestroy = true;
        
        //foreach(var key in keyword.CurrentRegisterKeyword)
        //{
        //    if(key.Key.KewordId == "AttachKeyword")
        //    {
        //        Managers.Effect.PlayEffect(Define.EFFECT.MonsterDeathEffect, this.transform);
        //        Managers.Sound.PlaySFX(Define.SOUND.ErrorEffectKeyword);
        //        keyword.DestroyKeywordEntity();
        //        Destroy(this);
        //        return;
        //    }
        //}
    }

    public override void StopLaserEvent()
    {
        keyword.IsDestroy = false;
    }

    public override void OffLaserHit()
    {
        hitcount--;
        if (hitcount <= 0)
        {
            hitcount = 0;
            if (!keyword.gameObject.activeSelf)
            {
                return;
            }
            StartCoroutine(MinusTime());
            StopLaserEvent();
        }
    }
}
