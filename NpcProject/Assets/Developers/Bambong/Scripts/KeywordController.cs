using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class KeywordController : UI_Base, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerExitHandler, IPointerEnterHandler
{
    private readonly float START_END_ANIM_TIME = 0.2f;
    private readonly float FOCUSING_SCALE = 1.05f;
    private readonly float KEYWORD_FRAME_MOVE_TIME = 0.1f;
    private readonly string KEYWORD_FRAME_TAG = "KeywordFrame";

    private bool isLock = false;

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;
    [SerializeField]
    private KeywordActionType keywordType;

    private int prevSibilintIndex;
    private Transform startParent;
    private Vector3 startDragPoint;
    private KeywordFrameBase curFrame;
    protected DebugZone parentDebugZone;
   
    public Image Image { get => image; }
    public string KewordId { get; private set; }
    public KeywordActionType KeywordType { get => keywordType; }
    public KeywordFrameBase CurFrame { get => curFrame;}
    public RectTransform RectTransform { get => rectTransform;  }

    private Color originColor;
    private readonly Color LOCK_COLOR = new Color(0.45f, 0.45f, 0.45f);

    private void Awake()
    {
         KewordId = GetType().ToString();
    }
    public void SetFrame(KeywordFrameBase frame) => curFrame = frame;
    public virtual void SetDebugZone(DebugZone zone) => parentDebugZone = zone;

    public void SetLock(bool isOn)
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLock) 
        {
            return;
        }

        if(eventData.button != PointerEventData.InputButton.Left) 
        {
            return;
        }
        curFrame.OnBeginDrag();
        prevSibilintIndex = rectTransform.GetSiblingIndex();
        startDragPoint = rectTransform.localPosition;
        startParent = transform.parent;
        transform.SetParent(Managers.Keyword.PlayerKeywordPanel.transform);
        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLock)
        {
            return;
        }
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLock)
        {
            return;
        }

        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        var raycasts = Managers.Keyword.GetRaycastList(eventData);

        if(raycasts.Count > 0) 
        {
            for(int i =0; i < raycasts.Count; ++i) 
            {
                if (raycasts[i].gameObject.CompareTag(KEYWORD_FRAME_TAG)) 
                {
                    var keywordFrame = raycasts[i].gameObject.GetComponent<KeywordFrameBase>();
                    if (curFrame == keywordFrame)
                    {
                        continue;
                    }

                    //if (keywordFrame.IsAvailable)
                    //{
                    keywordFrame.DragDropKeyword(this);
                    return;
                    //}
                    //else
                    //{
                    //    SwapKeywordFrame(keywordFrame);
                    //    return;
                    //}
                }
                else 
                {
                    Debug.Log(raycasts[i].gameObject.name);
                }
               
            }
        }
        ResetKeyword();
    }
 
    public void SwapKeywordFrame(KeywordFrameBase other) 
    {
        var innerKeyword = other.CurFrameInnerKeyword;
        curFrame.SetKeyWord(innerKeyword);
        other.SetKeyWord(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLock)
        {
            return;
        }
        transform.DOScale(FOCUSING_SCALE,START_END_ANIM_TIME).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLock)
        {
            return;
        }
        transform.DOScale(Vector3.one,START_END_ANIM_TIME).SetUpdate(true);
    }

    public DG.Tweening.Core.TweenerCore<Vector3,Vector3,DG.Tweening.Plugins.Options.VectorOptions> SetToKeywordFrame(Vector3 pos) 
    {
        return rectTransform.DOLocalMove(pos, KEYWORD_FRAME_MOVE_TIME).SetUpdate(true);
    }

    public void ResetKeyword()
    {
        if (gameObject == null)
        {
            return;
        }
        transform.SetParent(startParent);
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.DOLocalMove(startDragPoint,START_END_ANIM_TIME,true).SetUpdate(true).OnComplete(()=>curFrame.OnEndDrag());
    }
    public virtual void KeywordAction(KeywordEntity entity) 
    {
    }
    public virtual void OnRemove(KeywordEntity entity)
    { 
    }

    public override void Init()
    {
    }
}
