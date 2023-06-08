using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public abstract class KeywordFrameBase : UI_Base
{
   

    [SerializeField]
    protected RectTransform rectTransform;
    
    [SerializeField]
    private Image focusImage;

    [SerializeField]
    private Image[] frameColorImages;

    protected bool isLock;

    protected KeywordController curFrameInnerKeyword;
    private E_KEYWORD_TYPE availableKeywordType;

    private Vector2 focusDesireSize;
    private Vector2 focusStartSize;
    protected KeywordEntity masterEntity;
    

    public KeywordController CurFrameInnerKeyword { get => curFrameInnerKeyword; }
    public bool HasKeyword { get => CurFrameInnerKeyword != null; }
    public RectTransform RectTransform { get => rectTransform; }
    public E_KEYWORD_TYPE AvailableKeywordType { get => availableKeywordType; }
    public bool IsLock { get => isLock;  }


    private readonly float UNAVILABLE_COLOR_TIME = 0.15f;
    private readonly float START_END_ANIM_TIME = 0.2f;
    private readonly float FOCUSING_SCALE = 1.2f;

    private void Start()
    {
        focusStartSize = focusImage.rectTransform.sizeDelta;
        focusDesireSize = focusStartSize * FOCUSING_SCALE;
    }
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
    protected virtual void DecisionKeyword(KeywordController keyword) 
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
            Managers.Sound.PlaySFX(Define.SOUND.AssignmentKeyword);
            var prevCur = prevFrame.curFrameInnerKeyword;
            var mCur = curFrameInnerKeyword;
            prevFrame.SetKeyWord(mCur, ()=> {
                prevFrame.OnEndDrag();
                prevFrame.DecisionKeyword(mCur);

            });
            SetKeyWord(keywordController,
                () => 
                {
                    DecisionKeyword(keywordController);
                }
                );

            if (prevFrame.masterEntity != null)
            {
                prevFrame.masterEntity.RemoveAction(prevCur, mCur);
            }
            if (masterEntity != null)
            {
                masterEntity.RemoveAction(mCur, keywordController);
            }
        }
        else 
        {
            Managers.Sound.PlaySFX(Define.SOUND.ClickKeyword);
            keywordController.ResetKeyword();
        }
    }

    public void OnPointerEnter()
    {
        if (isLock)
        {
            return;
        }
        focusImage.rectTransform.DOKill();
        var animTime = START_END_ANIM_TIME * (1 - ((focusImage.rectTransform.sizeDelta.x- focusStartSize.x) / (focusDesireSize.x - focusStartSize.x)));
        focusImage.rectTransform.DOSizeDelta(focusDesireSize, animTime).SetUpdate(true);
    }
    public void OnPointerExit()
    {
        if (isLock)
        {
            return;
        }
        focusImage.rectTransform.DOKill();
        var animTime = START_END_ANIM_TIME * ((focusImage.rectTransform.sizeDelta.x - focusStartSize.x) / (focusDesireSize.x - focusStartSize.x));
        focusImage.rectTransform.DOSizeDelta(focusStartSize, animTime).SetUpdate(true);
    }
}
