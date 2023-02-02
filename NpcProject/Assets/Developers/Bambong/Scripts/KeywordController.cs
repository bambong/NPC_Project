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
    private readonly string KEYWORD_PLAYER_FRAME_TAG = "KeywordPlayerFrame";

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI keywordText;

    [SerializeField]
    private KeywordActionType keywordType;

    private int prevSibilintIndex;
    private Transform startParent;
    private Vector3 startDragPoint;
    private KeywordFrameController curFrame;
    public TextMeshProUGUI KeywordText { get => keywordText;}
    public Image Image { get => image; }
    public string KewordId { get; private set; }
    public KeywordActionType KeywordType { get => keywordType; }

    private void Awake()
    {
         KewordId = GetType().ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) 
        {
            return;
        }
        prevSibilintIndex = rectTransform.GetSiblingIndex();
        startParent = transform.parent;
        transform.parent = Managers.Keyword.PlayerKeywordPanel.transform;

        rectTransform.SetAsLastSibling();
        startDragPoint = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        var raycasts = Managers.Keyword.GetRaycastList(eventData);

        if(raycasts.Count > 0) 
        {
            for(int i =0; i < raycasts.Count; ++i) 
            {
                if(raycasts[i].gameObject.CompareTag(KEYWORD_FRAME_TAG)) 
                {
                    var keywordFrame = raycasts[i].gameObject.GetComponent<KeywordFrameController>();
                    if(keywordFrame.SetKeyWord(this)) 
                    {
                        curFrame = keywordFrame;
                        return;
                    }
                }
                else if(raycasts[i].gameObject.CompareTag(KEYWORD_PLAYER_FRAME_TAG))
                {
                    Managers.Keyword.AddKeywordToDebugZone(Managers.Keyword.CurDebugZone,this);
                    ClearCurFrame();
                    return;
                }
            }
        }
        ResetKeyword();
    }
 
    public void ClearCurFrame() 
    {
        if(curFrame != null) 
        {
            curFrame.ResetKeywordFrame();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(FOCUSING_SCALE,START_END_ANIM_TIME).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one,START_END_ANIM_TIME).SetUpdate(true);
    }

    public void SetToKeywordFrame(Vector3 pos) 
    {
        rectTransform.DOMove(pos, KEYWORD_FRAME_MOVE_TIME).SetUpdate(true);
    }

    public void ResetKeyword()
    {
        if (gameObject == null)
        {
            return;
        }
        transform.parent = startParent;
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.DOMove(startDragPoint,START_END_ANIM_TIME,true).SetUpdate(true);
    }
    public virtual void KeywordAction(KeywordEntity entity) 
    {
    }

    public override void Init()
    {
    }
}
