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


    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;

    private int prevSibilintIndex;
    private Transform startParent;
    private Vector3 startDragPoint;
    private KeywordFrameBase curFrame;
    protected DebugZone parentDebugZone;
    private bool isLock = false;
    
    public Image Image { get => image; }
    public string KewordId { get; private set; }
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
    private bool IsDragable(PointerEventData eventData) { return isLock || eventData.button != PointerEventData.InputButton.Left; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsDragable(eventData)) 
        {
            return;
        }
        Managers.Keyword.CurDragKeyword = this;
        curFrame.OnBeginDrag();
        prevSibilintIndex = rectTransform.GetSiblingIndex();
        startDragPoint = rectTransform.localPosition;
        startParent = transform.parent;
        transform.SetParent(Managers.Keyword.PlayerKeywordPanel.transform);
        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsDragable(eventData)|| Managers.Keyword.CurDragKeyword != this)
        {
            return;
        }
 
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsDragable(eventData) || Managers.Keyword.CurDragKeyword != this)
        {
            return;
        }
        var raycasts = Managers.Keyword.GetRaycastList(eventData);
        Managers.Keyword.CurDragKeyword = null;

        if (raycasts.Count > 0) 
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

                    keywordFrame.DragDropKeyword(this);
                    return;
                }
                else 
                {
                    Debug.Log(raycasts[i].gameObject.name);
                }
               
            }
        }
        ResetKeyword();
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
        isLock = true;
        transform.SetParent(startParent);
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.DOLocalMove(startDragPoint,START_END_ANIM_TIME,true).SetUpdate(true).OnComplete(
            ()=>
            { 
                curFrame.OnEndDrag();
                isLock = false;
            });
    }
    public void DragReset() 
    {
        transform.SetParent(startParent);
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.localPosition = startDragPoint;
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
    
    }

    public override void Init()
    {
        curFrame = null;
        parentDebugZone = null;
        if (isLock) 
        {
            SetLock(false);
        }
    }
}
