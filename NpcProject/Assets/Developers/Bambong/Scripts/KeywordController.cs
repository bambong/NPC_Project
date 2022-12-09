using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum E_KeywordType
{
    Action,
    Object
}

public  class KeywordController : MonoBehaviour, IDragHandler, IEndDragHandler,IBeginDragHandler ,IPointerExitHandler ,IPointerEnterHandler
{
    private readonly float START_END_ANIM_TIME = 0.2f;
    private readonly float FOCUSING_SCALE = 1.2f;
    private readonly float KEYWORD_FRAME_MOVE_TIME= 0.1f;
    private readonly string KEYWORD_FRAME_TAG = "KeywordFrame";

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;

    [SerializeField]
    private E_KeywordType keywordType;

    private string keywordId;
    private int prevSibilintIndex;
    private Coroutine curAnimCoroutine;
    private Vector3 startDragPoint;

    public E_KeywordType KeywordType { get => keywordType; }
    public string KeywordId { get => keywordId; }

    private void Awake()
    {
        keywordId = GetType().ToString();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) 
        {
            return;
        }
        prevSibilintIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
        startDragPoint = rectTransform.anchoredPosition;
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

        var raycasts = GraphicRayCasterManager.Instacne.GetRaycastList(eventData);

        if(raycasts.Count > 0) 
        {
            for(int i =0; i < raycasts.Count; ++i) 
            {
                if(raycasts[i].gameObject.CompareTag(KEYWORD_FRAME_TAG)) 
                {
                    KeywordManager.Instance.SetKeyWord(this);
                    return;
                }
            }
        }
        ResetKeyword();
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
        image.raycastTarget = false;
        rectTransform.DOMove(pos, KEYWORD_FRAME_MOVE_TIME).OnComplete(() => KeywordManager.Instance.Interaction());
    }

    public void ResetKeyword() 
    {
        if(gameObject == null) 
        {
            return;
        }
        rectTransform.DOAnchorPos(startDragPoint, START_END_ANIM_TIME).OnComplete(() =>
        { 
            image.raycastTarget = true;
            rectTransform.SetSiblingIndex(prevSibilintIndex);
        }); 
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

    public bool CompareKeywordType(E_KeywordType type)
    {
        return type == keywordType;
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

}
