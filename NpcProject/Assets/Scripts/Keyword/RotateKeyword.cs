using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RotateKeyword : KeywordController
{
    [SerializeField]
    private float rotateSpeed = 2f;
    public override void KeywordAction(KeywordEntity entity)
    {
        entity.ColisionCheckRotate(new Vector3(0,Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER) * rotateSpeed,0));
    }
}
