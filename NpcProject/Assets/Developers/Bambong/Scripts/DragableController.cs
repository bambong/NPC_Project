using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableController : MonoBehaviour, IDragHandler, IEndDragHandler,IBeginDragHandler ,IPointerExitHandler ,IPointerEnterHandler
{
    private readonly float START_END_ANIM_TIME = 0.2f;
    private readonly float FOCUSING_SCALE = 1.2f;
    private readonly float CELL_SIZE = 150f;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;

    private int prevSibilintIndex;
    private Coroutine curAnimCoroutine;
    private Vector3 startDragPoint;
    
    public void OnBeginDrag(PointerEventData eventData)
    {

        prevSibilintIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
        startDragPoint = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var raycasts = GraphicRayCasterManager.Instacne.GetRaycastList(eventData);

        if(raycasts.Count > 0) 
        {
            for(int i =0; i < raycasts.Count; ++i) 
            {
                if(raycasts[i].gameObject.CompareTag("KeywordFrame")) 
                {
                    var keywordController = raycasts[i].gameObject.GetComponent<KeywordFameController>();
                    if(!keywordController.SetKeyWord(this)) 
                    {
                       ResetKeyword();
                    }
  
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
        rectTransform.position = pos;
    }

    public void ResetKeyword() 
    {
        image.raycastTarget = true;
        rectTransform.anchoredPosition = startDragPoint;
        rectTransform.SetSiblingIndex(prevSibilintIndex);
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


}
