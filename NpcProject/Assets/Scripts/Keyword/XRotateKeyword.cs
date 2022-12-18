using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class XRotateKeyword : KeywordController
{
    [SerializeField]
    private float rotateSpeed = 2f;
    public override void KeywordUpdateAction(KeywordEntity entity)
    {
        var rot = entity.transform.localEulerAngles;
        rot.y += Time.deltaTime * rotateSpeed;
        entity.transform.Rotate(new Vector3(0,Time.deltaTime * rotateSpeed,0));
    }
    public override void EnterKeywordAction(KeywordEntity entity)
    {
        //entity.transform.DORotate(new Vector3(0,360,0),2f).SetLoops(-1,LoopType.Incremental).SetEase(Ease.Linear);
    }
    public override void ExitKeywordAction(KeywordEntity entity)
    {
        //entity.transform.DOKill();
    }
}
