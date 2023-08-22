using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

[Flags]
public enum E_KEYWORD_TYPE
{
    ALL = -1,
    DEFALUT = 1 << 0,
    ATTACK = 1 << 1,
}
public class KeywordController : UI_Base
{

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;
    [SerializeField]
    private E_KEYWORD_TYPE keywordType = E_KEYWORD_TYPE.DEFALUT;
    [SerializeField]
    private int needMakeGauge = 3;
    private int prevSibilintIndex;
    private Transform startParent;
    private Vector3 startDragPoint;
    private KeywordFrameBase curFrame;
    protected DebugZone parentDebugZone;

    private bool isInit = false;
    private bool isLock = false;
    private bool isMove = false;
    private bool isDrag = false;
    public bool isPlay = false;
    public Image Image { get => image; }
    public string KeywordId { get; private set; }
    public KeywordFrameBase CurFrame { get => curFrame;}
    public RectTransform RectTransform { get => rectTransform;  }
    public E_KEYWORD_TYPE KeywordType { get => keywordType;  }
    public bool IsLock { get => isLock; }
    public int NeedMakeGauge { get => needMakeGauge;  }

    private Color originColor;
   
    private readonly float START_END_ANIM_TIME = 0.2f;
    private readonly float FOCUSING_SCALE = 1.15f;
    private readonly float KEYWORD_FRAME_MOVE_TIME = 0.1f;

    private readonly Color LOCK_COLOR = new Color(0.45f, 0.45f, 0.45f);
 

    private void Awake()
    {
         KeywordId = GetType().ToString();
    }

    public void SetFrame(KeywordFrameBase frame)
    {
        curFrame = frame;
    }
    public virtual void SetDebugZone(DebugZone zone) => parentDebugZone = zone;

    public void SetLockState(bool isOn)
    {
        if (isOn)
        {
            originColor = image.color;
            image.color = LOCK_COLOR;
        }
        else 
        {
            image.color = originColor;
        }
        isLock = isOn;
    }
    public void SetMoveState(bool isOn)
    {
        isMove = isOn;
    }
    public void SetDragState(bool isOn)
    {
        isDrag = isOn;
    }

    private bool IsDragable() { return isLock; }
   
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsDragable() || isMove) 
        {
            return;
        }
        Debug.Log("현재 드래그 중");
        Managers.Sound.PlaySFX(Define.SOUND.ClickKeyword);

        SetDragState(true);
        SetMoveState(true);
        Managers.Keyword.CurDragKeyword = this;
        curFrame.OnBeginDrag();
        prevSibilintIndex = rectTransform.GetSiblingIndex();
        startDragPoint = curFrame.RectTransform.localPosition;
        startParent = transform.parent;
        transform.SetParent(Managers.Keyword.PlayerKeywordPanel.transform);
        rectTransform.SetAsLastSibling();
        //StartCoroutine(DragCheck());
    }
   
    public void OnDrag(PointerEventData eventData)
    {
        if (IsDragable()|| Managers.Keyword.CurDragKeyword != this)
        {
            return;
        }
 
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(KeywordFrameBase nearFrame)
    {
        if (IsDragable())
        { 
            return;
        }
        SetDragState(false);
        Managers.Keyword.CurDragKeyword = null;
         
        if (nearFrame != null) 
        {
            nearFrame.DragDropKeyword(this);
            return;
        }

        Managers.Sound.PlaySFX(Define.SOUND.ToDropKeyword);
        ResetKeyword();
    }

    public void OnPointerEnter()
    {
        if (isLock || isMove)
        {
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.ButtonHover);
        transform.DOKill();
        var animTime = START_END_ANIM_TIME * (1 - ((transform.localScale.x - 1) / (FOCUSING_SCALE - 1)));
        transform.DOScale(FOCUSING_SCALE, animTime);
       // rectTransform.localScale = new Vector3(FOCUSING_SCALE, FOCUSING_SCALE, 1);
    }
    public void OnPointerExit()
    {
        if (isLock || isMove)
        {
            return;
        }
        transform.DOKill();
        var animTime = START_END_ANIM_TIME * ((transform.localScale.x - 1) / (FOCUSING_SCALE - 1));
        transform.DOScale(Vector3.one, animTime);
        //rectTransform.localScale = Vector3.one;
    }
    public DG.Tweening.Core.TweenerCore<Vector3,Vector3,DG.Tweening.Plugins.Options.VectorOptions> SetToKeywordFrame(Vector3 pos) 
    {
        return rectTransform.DOLocalMove(pos, KEYWORD_FRAME_MOVE_TIME);
    }

    public void ResetKeyword()
    {
        if (gameObject == null)
        {
            return;
        }
        SetMoveState(true);
        transform.SetParent(startParent);
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.DOLocalMove(startDragPoint,START_END_ANIM_TIME,true).SetUpdate(true).OnComplete(
            ()=>
            { 
                curFrame.OnEndDrag();
                SetMoveState(false);
                rectTransform.localScale = Vector3.one;
            });
    }
    public void DragReset() 
    {
        transform.SetParent(startParent);
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.localPosition = startDragPoint;
        rectTransform.localScale = Vector3.one;
        SetMoveState(false);
        SetDragState(false);
        curFrame.OnEndDrag();
    }
  
    public virtual void OnEnter(KeywordEntity entity)
    {
    }
    public virtual void OnFixedUpdate(KeywordEntity entity) 
    {
    }
    public virtual void OnUpdate(KeywordEntity entity)
    {
    }

    public virtual void OnRemove(KeywordEntity entity)
    { 
    }
    public virtual void ClearForPool() 
    {
        isInit = false;
        curFrame.ResetKeywordFrame();
        StopAllCoroutines();
        //rectTransform.DOKill();
    }
    public void DestroyKeyword() 
    {
        if (!isInit)
        {
            return;
        }
        ClearForPool();
        Managers.Resource.Destroy(gameObject);
    }

    public override void Init()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;
        curFrame = null;
        SetDragState(false);
        SetMoveState(false);
        parentDebugZone = null;
        if (isLock) 
        {
            SetLockState(false);
        }
    }
}
