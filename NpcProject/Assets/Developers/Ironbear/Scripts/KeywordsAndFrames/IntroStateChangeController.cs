using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class IntroStateChangeController : UI_Base, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private readonly float START_END_ANIM_TIME = 0.2f;

    [SerializeField]
    private RectTransform rectTransform;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private int prevSibilintIndex;
    private Transform startParent;
    private Vector3 startDragPoint;
    private KeywordFrameBase curFrame;

    private float moveAnimDuration = 0.2f;

    private bool isInit = false;
    private bool isLock = false;

    private void Start()
    {
        startDragPoint = GetComponent<RectTransform>().localPosition;
        canvasGroup = GetComponent<CanvasGroup>();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
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

    private bool IsDragable(PointerEventData eventData) { return isLock || eventData.button != PointerEventData.InputButton.Left; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Managers.Sound.PlaySFX(Define.SOUND.ClickButton);
        startParent = transform.parent;

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
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
            });
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == canvas.transform)
        {
            Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleBad);
            ResetPosition();
        }
        else
        {
            Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleGood);
        }

        canvasGroup.blocksRaycasts = true;        
    }

    private void ResetPosition()
    {
        transform.DOLocalMove(startDragPoint, moveAnimDuration).SetEase(Ease.OutQuart);
        transform.SetParent(startParent);
    }


}