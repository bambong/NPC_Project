using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class KeywordFrameBase : UI_Base
{
    [SerializeField]
    protected RectTransform rectTransform;

    [SerializeField]
    private Image[] frameColorImages;

    protected KeywordController curFrameInnerKeyword;
    private E_KEYWORD_TYPE availableKeywordType;
    public KeywordController CurFrameInnerKeyword { get => curFrameInnerKeyword; }
    public bool HasKeyword { get => CurFrameInnerKeyword != null; }
    public RectTransform RectTransform { get => rectTransform; }
    public E_KEYWORD_TYPE AvailableKeywordType { get => availableKeywordType; }
    private readonly float UNAVILABLE_COLOR_TIME = 0.15f;

    public virtual void SetKeyWord(KeywordController keywordController,Action onComplete = null) 
    {
        if (keywordController == null)
        {
            ResetKeywordFrame();
            onComplete?.Invoke();
            return;
        }

        keywordController.SetFrame(this);
        curFrameInnerKeyword = keywordController;
        keywordController.transform.SetParent(transform);
        
        keywordController.SetToKeywordFrame(rectTransform.localPosition).OnComplete(() =>
        {
            curFrameInnerKeyword.SetMoveState(false);
            onComplete?.Invoke();
        });

    }
    public void SetKeywordType(E_KEYWORD_TYPE availableKeywordType)
    {
        this.availableKeywordType= availableKeywordType;
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

        if (!IsAvailableKeyword(keywordController)) 
        {
            Debug.LogError("불가능한 타입의 키워드 생성");
            return;
        }
        keywordController.SetFrame(this);
        curFrameInnerKeyword = keywordController;
        keywordController.transform.SetParent(transform);
        keywordController.RectTransform.position = rectTransform.position;
    }
    protected virtual void DecisionKeyword() 
    {
    }
    protected void SetFrameColor(Color color) 
    {
        for (int i = 0; i < frameColorImages.Length; ++i)
        {
            frameColorImages[i].color = color;
        }
    }

    public bool IsAvailableKeyword(KeywordController keywordController) 
    {
        if(keywordController== null) 
        {
            return true;
        }
        var bitMask = keywordController.KeywordType & availableKeywordType;
        if(0 != bitMask) 
        {
            return true;
        }
        else 
        {
            for (int i = 0; i < frameColorImages.Length; ++i)
            {
                frameColorImages[i].DOColor(Color.red, UNAVILABLE_COLOR_TIME).SetLoops(2,LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
            return false;
        }
    }

    protected void ClearDotween() 
    {
        for (int i = 0; i < frameColorImages.Length; ++i)
        {
            frameColorImages[i].DOKill();
        }
    }
    public void DragDropKeyword(KeywordController keywordController)
    {
        var prevFrame = keywordController.CurFrame;
        if (IsAvailableKeyword(keywordController) && prevFrame.IsAvailableKeyword(CurFrameInnerKeyword)) 
        {
            Managers.Sound.AskSfxPlay(20010);
            prevFrame.SetKeyWord(curFrameInnerKeyword, prevFrame.OnEndDrag);
            SetKeyWord(keywordController, DecisionKeyword);
        }
        else 
        {
            Managers.Sound.AskSfxPlay(20012);
            keywordController.ResetKeyword();
        }
    }
}
