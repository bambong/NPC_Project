using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    private E_KEYWORD_TYPE keywordType = E_KEYWORD_TYPE.DEFALUT; // 키워드 타입
    [SerializeField]
    private int needMakeGauge = 3;

    private int prevSibilintIndex;
    private Transform startParent;
    private Vector3 startDragPoint;
    private KeywordFrameBase curFrame;
    protected DebugZone parentDebugZone;
    private Color originColor;

    private bool isInit = false;
    private bool isLock = false;
    private bool isMove = false;
    public bool isPlay = false;

    public Image Image { get => image; }
    public string KeywordId { get; private set; }
    public KeywordFrameBase CurFrame { get => curFrame;}
    public RectTransform RectTransform { get => rectTransform;  }
    public E_KEYWORD_TYPE KeywordType { get => keywordType;  }
    public bool IsLock { get => isLock; }
    public int NeedMakeGauge { get => needMakeGauge;  }

    private readonly float START_END_ANIM_TIME = 0.2f;
    private readonly float FOCUSING_SCALE = 1.15f;
    private readonly float KEYWORD_FRAME_MOVE_TIME = 0.1f;
    private readonly Color LOCK_COLOR = new Color(0.45f, 0.45f, 0.45f);
 

    private void Awake()
    {
         KeywordId = GetType().ToString();
    }

    public void SetFrame(KeywordFrameBase frame) // 현재 프레임 설정
    {
        curFrame = frame;
    }
    public virtual void SetDebugZone(DebugZone zone) => parentDebugZone = zone;

    public void SetLockState(bool isOn) // 잠금 상태 설정
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
  

    private bool IsDragable() { return isLock; }
   
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsDragable() || isMove) 
        {
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.ClickKeyword); // 드래그 시작 사운드 

        SetMoveState(true); // 움직이는 상태 설정
        Managers.Keyword.CurDragKeyword = this ; // 현재 드래그 중인 키워드로 등록
        curFrame.OnBeginDrag(); // 현재 키워드 프레임에게도 드래그 시작 이벤트 호출
        prevSibilintIndex = rectTransform.GetSiblingIndex();// 이전 하이라커 순서 저장
        startDragPoint = curFrame.RectTransform.localPosition;  // 드래그 시작 위치 저장
        startParent = transform.parent; // 현재 하이라커상 부모를 저장 
        transform.SetParent(Managers.Keyword.PlayerKeywordPanel.transform);// 그려지는 순서를 위해 부모 변경
        rectTransform.SetAsLastSibling(); // 가장 우선 순위를 높여 보이도록
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
        Managers.Keyword.CurDragKeyword = null;
         
        if (nearFrame != null)  //프레임을 전달 받았다면 실행
        {
            nearFrame.DragDropKeyword(this);
            return;
        }

        Managers.Sound.PlaySFX(Define.SOUND.ToDropKeyword);
        ResetKeyword();
    }

    public void OnPointerEnter() // 포인터가 안으로 들어왔을 때 애니메이션 재생  
    {
        if (isLock || isMove)
        {
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.ButtonHover);
        transform.DOKill();
        var animTime = START_END_ANIM_TIME * (1 - ((transform.localScale.x - 1) / (FOCUSING_SCALE - 1)));
        transform.DOScale(FOCUSING_SCALE, animTime);
    }
    public void OnPointerExit()// 포인터가 밖으로 나갔을 때 애니메이션 재생  
    {
        if (isLock || isMove)
        {
            return;
        }
        transform.DOKill();
        var animTime = START_END_ANIM_TIME * ((transform.localScale.x - 1) / (FOCUSING_SCALE - 1));
        transform.DOScale(Vector3.one, animTime);
    }
    public DG.Tweening.Core.TweenerCore<Vector3,Vector3,DG.Tweening.Plugins.Options.VectorOptions> SetToKeywordFrame(Vector3 pos) 
    {
        return rectTransform.DOLocalMove(pos, KEYWORD_FRAME_MOVE_TIME);
    }

    public void ResetKeyword() // 애니메이션과 함께 키워드 원래 위치로 복구
    {
        if (gameObject == null)
        {
            return;
        }
        SetMoveState(true);
        transform.SetParent(startParent);
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.DOLocalMove(startDragPoint,START_END_ANIM_TIME,true).SetUpdate(true).OnComplete( //원래 위치로 돌아온 후 이벤트 발생
            ()=>
            { 
                curFrame.OnEndDrag(); // 현재 프레임 이벤트 호출
                SetMoveState(false); // 다시 움직일 수 있는 상태로 변경 
                rectTransform.localScale = Vector3.one;
            });
    }
    public void DragReset() // 애니메이션 없이 키워드 리셋
    {
        transform.SetParent(startParent);
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.localPosition = startDragPoint;
        rectTransform.localScale = Vector3.one;
        SetMoveState(false);
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
        SetMoveState(false);
        parentDebugZone = null;
        if (isLock) 
        {
            SetLockState(false);
        }
    }
}
