using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class XRotateKeyword : KeywordController
{
    [SerializeField]
    private float rotateSpeed = 2f;
    public override void KeywordAction(KeywordEntity entity)
    {
        var rot = entity.transform.localEulerAngles;
        rot.y += Time.deltaTime * rotateSpeed;
        entity.transform.Rotate(new Vector3(0,Time.deltaTime * rotateSpeed,0));
    }
}
