using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;

public class MovePlatformController : KeywordEntity
{
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private Vector3 scaleDesire;

    [SerializeField]
    private MMFeedbacks onScaleFeedback;

    private Vector3 prevScale;
    private Vector3 originScale;
    public override Transform KeywordTransformFactor => parent;
    private void Awake()
    {
        originScale = transform.lossyScale;
        prevScale = parent.transform.localScale;
       // AddOverrideTable(typeof(ScaleKeyword).ToString(),new KeywordAction(OnScaleKeyword,KeywordActionType.OneShot,null));
    }
    //public void OnScaleKeyword(KeywordEntity entity) 
    //{
    //    if (transform.lossyScale.magnitude < scaleDesire.magnitude)
    //    {
    //        //speed = InSine(transform.lossyScale.magnitude / targetScale.magnitude)*10;
    //        var curFrameDesirScale = transform.lossyScale + (originScale * Time.deltaTime * 10);
    //        if (curFrameDesirScale.magnitude > scaleDesire.magnitude)
    //        {
    //            curFrameDesirScale = scaleDesire;
    //        }
    //        entity.ColisionCheckScale(scaleDesire,ScaleKeyword)
    //    }
    //    //onScaleFeedback.PlayFeedbacks();
    //}

}



