using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeywordFrameBase : UI_Base
{
    [SerializeField]
    protected RectTransform rectTransform;

    protected KeywordController curFrameInnerKeyword;
    public KeywordController CurFrameInnerKeyword { get => curFrameInnerKeyword; }
    public bool HasKeyword { get => CurFrameInnerKeyword != null; }
    public RectTransform RectTransform { get => rectTransform; }

    public virtual void SetKeyWord(KeywordController keywordController,Action onComplete = null) 
    {
        if (keywordController == null)
        {
            ResetKeywordFrame();
            onComplete?.Invoke();
            return;
        }
        //else 
        //{
        //    CurFrameInnerKeyword.SetFrame(keywordController.CurFrame);
        //}
        //var prevFrame = keywordController.CurFrame;
        //prevFrame.SetKeyWord(CurFrameInnerKeyword);
        keywordController.SetFrame(this);
        curFrameInnerKeyword = keywordController;
        keywordController.transform.SetParent(transform);
        
        keywordController.SetToKeywordFrame(rectTransform.localPosition).OnComplete(() =>
        {
            curFrameInnerKeyword.SetMoveState(false);
            onComplete?.Invoke();
        });
        //keywordController.SetToKeywordFrame(rectTransform.localPosition).OnComplete(() => 
        //{ 
        //    prevFrame.OnEndDrag();
        //    entity.DecisionKeyword(this);
        //});
    }
    public virtual void ResetKeywordFrame()
    {
        curFrameInnerKeyword = null;
    }

    public virtual void OnBeginDrag() { }
    public virtual void OnEndDrag() { }
    public override void Init(){}

    public void InitKeyword(KeywordController keywordController)
    {
        keywordController.SetFrame(this);
        curFrameInnerKeyword = keywordController;
        keywordController.transform.SetParent(transform);
        keywordController.RectTransform.position = rectTransform.position;
    }
    protected virtual void DecisionKeyword() 
    {
    }
    public void DragDropKeyword(KeywordController keywordController)
    {
        var prevFrame = keywordController.CurFrame;
        prevFrame.SetKeyWord(curFrameInnerKeyword, prevFrame.OnEndDrag);
        SetKeyWord(keywordController, DecisionKeyword);
    }
}
