using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;

public class IntroKeywordController : UI_Base, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private readonly float START_END_ANIM_TIME = 0.2f;

    [SerializeField]
    private RectTransform rectTransform;

    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private CanvasGroup canvasGroup;
    private int prevSibilintIndex;
    private Transform startParent;
    private Vector3 startDragPoint;
    private KeywordFrameBase curFrame;

    private float moveAnimDuration = 0.2f;

    private bool isInit = false;
    private bool isMove = false;
    private bool isDrag = false;
    private bool isLock = false;
    private Vector2 offset;

    private void Start()
    {
        startDragPoint = GetComponent<RectTransform>().localPosition;
        canvasGroup = GetComponent<CanvasGroup>();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    public override void Init()
    {
        if (isInit)
        {
            return;
        }
    }

    public void SetFrame(KeywordFrameBase frame)
    {
        curFrame = frame;
    }

    public void SetMoveState(bool isOn)
    {
        isMove = isOn;
    }
    public void SetDragState(bool isOn)
    {
        isDrag = isOn;
    }

    private bool IsDragable(PointerEventData eventData) { return isLock || eventData.button != PointerEventData.InputButton.Left; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        offset = (Vector2)rectTransform.position - (Vector2)Input.mousePosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 vec = Camera.main.WorldToScreenPoint(rectTransform.position);
        vec.x += eventData.delta.x;
        vec.y += eventData.delta.y;
        rectTransform.position = Camera.main.ScreenToWorldPoint(vec);
    }

    public void ResetKeyword()
    {
        if (gameObject == null)
        {
            return;
        }

        transform.SetParent(startParent);
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.DOLocalMove(startDragPoint, START_END_ANIM_TIME, true).SetUpdate(true)
            .OnComplete(() =>
            {
                curFrame.OnEndDrag();
                SetMoveState(false);
            });
    }

    public void OnEndDrag(PointerEventData eventData)
    { 
        //키워드 칸에 못 들어갔을 때
        if (transform.parent == canvas.transform)
        {
            transform.DOLocalMove(startDragPoint, moveAnimDuration).SetEase(Ease.OutQuart);
            transform.SetParent(startParent);
        }

        canvasGroup.blocksRaycasts = true;

        SetDragState(false);
    }
}