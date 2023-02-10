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
    private void Awake()
    {
        prevScale = parent.transform.localScale;
        AddOverrideTable(typeof(ScaleKeyword).ToString(),new KeywordAction(OnScaleKeyword,KeywordActionType.OneShot,OnScaleKeywordRemove));
    }
    public void OnScaleKeyword(KeywordEntity entity) 
    {
        onScaleFeedback.PlayFeedbacks();
        parent.DOScale(scaleDesire,1).SetEase(Ease.OutCirc);
    }
    public void OnScaleKeywordRemove(KeywordEntity entity) 
    {
        parent.DOScale(prevScale,0.5f).SetEase(Ease.OutCirc);

    }
}



