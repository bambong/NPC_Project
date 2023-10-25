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
    [SerializeField]
    private Color hasKeywordColor;

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
    private readonly float FOCUSING_SCALE = 1.6f;

    private void Start()
    {
        focusStartSize = focusImage.rectTransform.sizeDelta;
        focusDesireSize = focusStartSize * FOCUSING_SCALE;
    }
    public virtual void SetKeyWord(KeywordController keywordController,Action onComplete = null) 
    {
        if (keywordController == null) // 키워드가 안들어왔다면 초기화
        {
            SetFrameColor(Color.white);
            ResetKeywordFrame(); 
            onComplete?.Invoke();
            return;
        }
        SetFrameColor(hasKeywordColor); // 키워드가 들어왔다면 키워드 보유 상태 UI 이미지 색상을 변경
        keywordController.SetFrame(this); // 키워드에게 현재 프레임 등록
        curFrameInnerKeyword = keywordController; // 현재 키워드 설정
        keywordController.transform.SetParent(transform);  
        
        keywordController.SetToKeywordFrame(rectTransform.localPosition).OnComplete(() => // 키워드 프레임 중심으로 이동 애니메이션 재생 완료시
        {
            curFrameInnerKeyword.SetMoveState(false);  // 키워드 움직임 상태 해제
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

    public void InitKeyword(KeywordController keywordController) // 키워드를 처음부터 생성할 경우 등록
    {

        if (!IsAvailableKeyword(keywordController)) // 타입에 맞지 않는 키워드 생성시 반환
        {
            Debug.LogError("불가능한 타입의 키워드 생성");
            return;
        }
        SetFrameColor(hasKeywordColor); 
        keywordController.SetFrame(this);
        curFrameInnerKeyword = keywordController;
        keywordController.transform.SetParent(transform);
        keywordController.transform.localScale = Vector3.one;
        keywordController.RectTransform.position = rectTransform.position; 
    }
    protected virtual void DecisionKeyword(KeywordController keyword) // 키워드가 등록되었을 때 호출
    {
    }
    protected void SetFrameColor(Color color) 
    {
        for (int i = 0; i < frameColorImages.Length; ++i)
        {
            frameColorImages[i].color = color;
        }
    }

    public bool IsAvailableKeyword(KeywordController keywordController)  // 들어올 수 있는 적합한 키워드인지 확인
    {
        if(keywordController== null) 
        {
            return true;
        }
        var bitMask = keywordController.KeywordType & availableKeywordType; // 비트 마스크 연산  
        if(0 != bitMask)  // 해당되는 타입이 존재한다면 True 반환
        {
            return true;
        }
        else  // 존재하지 않는다면 Color 를 변경시켜 불가능하다는 것을 알리고 False 반환
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
    public void ForceMoveKeyword(KeywordController keywordController) 
    {
            var prevFrame = keywordController.CurFrame;
     
            Managers.Sound.PlaySFX(Define.SOUND.AssignmentKeyword);
            var prevCur = prevFrame.curFrameInnerKeyword;
            var mCur = curFrameInnerKeyword;

            prevFrame.SetKeyWord(null, () => {
                prevFrame.DecisionKeyword(null);
            });

            if (prevFrame.masterEntity != null)
            {
                prevFrame.masterEntity.RemoveAction(prevCur, mCur);
            }
            if (masterEntity != null)
            {
                masterEntity.RemoveAction(mCur, keywordController);
            }

            SetFrameColor(hasKeywordColor);
            keywordController.SetFrame(this);
            curFrameInnerKeyword = keywordController;
            keywordController.transform.SetParent(transform);
            keywordController.transform.localPosition = Vector3.zero;
            DecisionKeyword(keywordController);
    }
    public void DragDropKeyword(KeywordController keywordController) // 키워드가 해당 프레임에 Drop 되었을 때 호출 (스왑 포함) 
    {
        var prevFrame = keywordController.CurFrame;
        if (IsAvailableKeyword(keywordController) && prevFrame.IsAvailableKeyword(CurFrameInnerKeyword))  // 들어온 키워드 현재 키워드 스왑 가능 여부 확인 (키워드가 없다면 무조건 가능)   
        {
            Managers.Sound.PlaySFX(Define.SOUND.AssignmentKeyword); 
            var prevCur = prevFrame.curFrameInnerKeyword;
            var mCur = curFrameInnerKeyword;
            prevFrame.SetKeyWord(mCur, ()=> {
                prevFrame.OnEndDrag();
                prevFrame.DecisionKeyword(mCur);
            });
            SetKeyWord(keywordController, // 키워드를 등록하고
                () => 
                {
                    DecisionKeyword(keywordController); // 키워드 움직이는 애니메이션이 끝나면 이벤트 발생
                }
                );

            if (prevFrame.masterEntity != null)  
            {
                prevFrame.masterEntity.RemoveAction(prevCur, mCur); // 프레임이 등록된 수신 오브젝트가 있다면 액션을 삭제
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

    public void OnPointerEnter() // 마우스가 포커싱되면 애니메이션 재생 
    {
        if (isLock)
        {
            return;
        }
        focusImage.rectTransform.DOKill();
        var animTime = START_END_ANIM_TIME * (1 - ((focusImage.rectTransform.sizeDelta.x- focusStartSize.x) / (focusDesireSize.x - focusStartSize.x)));
        focusImage.rectTransform.DOSizeDelta(focusDesireSize, animTime).SetUpdate(true);
    }
    public void OnPointerExit() // 마우스가 포커싱이 해제되면 애니메이션 재생 
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
