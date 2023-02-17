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
        entity.ColisionCheckRotate(new Vector3(0,Time.deltaTime * rotateSpeed,0));
        //entity.KeywordTransformFactor.Rotate(new Vector3(0,Time.deltaTime * rotateSpeed,0));
    }
}
