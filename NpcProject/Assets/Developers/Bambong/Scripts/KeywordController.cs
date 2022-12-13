using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class KeywordController : UI_Base, IDragHandler, IEndDragHandler,IBeginDragHandler ,IPointerExitHandler ,IPointerEnterHandler
{
    private readonly float START_END_ANIM_TIME = 0.2f;
    private readonly float FOCUSING_SCALE = 1.2f;
    private readonly float KEYWORD_FRAME_MOVE_TIME= 0.1f;
    private readonly string KEYWORD_FRAME_TAG = "KeywordFrame";
    private readonly string KEYWORD_PLAYER_FRAME_TAG = "KeywordPlayerFrame";

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;

    private int prevSibilintIndex;
    private Coroutine curAnimCoroutine;
    private Transform startParent; 
    private Vector3 startDragPoint;
    private KeywordFameController curFrame;
 
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
                    var keywordFrame = raycasts[i].gameObject.GetComponent<KeywordFameController>();
                    if(keywordFrame.SetKeyWord(this)) 
                    {
                        Managers.Keyword.RemoveKeywordToPlayer(this);
                        curFrame = keywordFrame;
                        return;
                    }
                }
                else if(raycasts[i].gameObject.CompareTag(KEYWORD_PLAYER_FRAME_TAG))
                {
                    if(!Managers.Keyword.AddKeywordToPlayer(this)) 
                    {
                        ResetKeyword();
                    }
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
        ClearAnim();
        curAnimCoroutine = StartCoroutine(ChangeScaleLerpAnim(new Vector3(FOCUSING_SCALE, FOCUSING_SCALE, FOCUSING_SCALE), START_END_ANIM_TIME));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ClearAnim();
        curAnimCoroutine = StartCoroutine(ChangeScaleLerpAnim(Vector3.one, START_END_ANIM_TIME));  
    }

    public void ClearAnim() 
    {
        if(curAnimCoroutine != null) 
        {
            StopCoroutine(curAnimCoroutine);
        }
    }

    public void SetToKeywordFrame(Vector3 pos) 
    {
        //image.raycastTarget = false;
        rectTransform.position = pos;
        //rectTransform.DOMove(pos, KEYWORD_FRAME_MOVE_TIME);
    }

    public void ResetKeyword() 
    {
        if(gameObject == null) 
        {
            return;
        }
        transform.parent = startParent;
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.transform.position = startDragPoint;
        //rectTransform.DOMove(startDragPoint,START_END_ANIM_TIME);
        //rectTransform.DOAnchorPos(startDragPoint, START_END_ANIM_TIME).OnComplete(() =>
        //{ 
        //    image.raycastTarget = true;
        //    rectTransform.SetSiblingIndex(prevSibilintIndex);
        //}); 
    }
    public IEnumerator ChangeScaleLerpAnim(Vector3 desireScale, float time) 
    {
        float progress = 0f;
        float revisionNum = 1 / time;
        var startScale = rectTransform.localScale;

        while (progress < 1) 
        {
            yield return null;
            progress += Time.deltaTime * revisionNum;
            rectTransform.localScale = Vector3.Lerp(startScale, desireScale, progress); 
        }
        rectTransform.localScale = desireScale;
    }

    public virtual bool HandleObjectKeyword(KeywordController objectKeywordController)
    {
        return false;
    }
    public void Remove()
    {
        ClearAnim();
        rectTransform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { Destroy(rectTransform.gameObject); });
        
    }
    public virtual void KeywordUpdateAction(KeywordEntity entity) 
    {
    }

    public override void Init()
    {
    }
}
