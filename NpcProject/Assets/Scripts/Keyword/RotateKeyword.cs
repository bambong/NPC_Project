using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RotateKeyword : KeywordController
{
    [SerializeField]
    private float rotateSpeed = 2f;

    public override void OnEnter(KeywordEntity entity)
    {
        if (isPlay)
        {
            Managers.Sound.PlaySFX(Define.SOUND.RotatingKeyword);
        }
        isPlay = true;
    }
    public override void OnFixedUpdate(KeywordEntity entity)
    {
        entity.ColisionCheckRotate(new Vector3(0,Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER) * rotateSpeed,0));
    }
    public override void OnRemove(KeywordEntity entity)
    {
        Managers.Sound.StopSFX();
    }
}
