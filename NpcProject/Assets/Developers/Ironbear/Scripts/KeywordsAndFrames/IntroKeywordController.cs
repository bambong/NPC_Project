using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;

public class IntroKeywordController : UI_Base, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler
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
    private bool sfxSoundPlay = true;

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
        startParent = transform.parent;

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
        
        Managers.Sound.PlaySFX(Define.SOUND.ClickButton);
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
            });
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == canvas.transform)
        {
            canvasGroup.blocksRaycasts = false;
            transform.SetParent(startParent);
            Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleBad);
            transform.DOLocalMove(startDragPoint, moveAnimDuration).SetEase(Ease.OutQuart).OnComplete(() =>
            {                                
                canvasGroup.blocksRaycasts = true;
            });           
        }        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Managers.Sound.PlaySFX(Define.SOUND.ButtonHover);
    }

}