using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MovePlatformController : KeywordEntity
{
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private Vector3 scaleDesire;
    private Vector3 prevScale;
    private void Awake()
    {
        prevScale = parent.transform.localScale;
        AddOverrideTable(typeof(ScaleKeyword).ToString(),new KeywordAction(OnScaleKeyword,KeywordActionType.OneShot,OnScaleKeywordRemove));
    }
    public void OnScaleKeyword(KeywordEntity entity) 
    {
        parent.DOScale(scaleDesire,1);
    }
    public void OnScaleKeywordRemove(KeywordEntity entity) 
    {
        parent.DOScale(prevScale,1);
    }
}



