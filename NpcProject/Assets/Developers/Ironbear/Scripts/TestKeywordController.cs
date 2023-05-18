using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class TestKeywordController : UI_Base, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerExitHandler, IPointerEnterHandler
{
    private readonly float START_END_ANIM_TIME = 0.2f;
    private readonly string KEYWORD_FRAME_TAG = "KeywordFrame";

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Transform canvas;

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


    private void Start()
    {
        startDragPoint = GetComponent<RectTransform>().localPosition;
        canvasGroup = GetComponent<CanvasGroup>();
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

        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;

        //prevSibilintIndex = rectTransform.GetSiblingIndex();
        /*
        if(IsDragable(eventData)||isMove)
        {
            return;
        }
        //Managers.Sound.AskSfxPlay(20009); -> 자꾸 씬 로딩 화면이 뜸

        SetDragState(true);
        SetMoveState(true);

        //Managers.Keyword.CurDragKeyword = this;
        curFrame.OnBeginDrag();

        prevSibilintIndex = rectTransform.GetSiblingIndex();
        startDragPoint = curFrame.RectTransform.localPosition;
        startParent = transform.parent;

        transform.SetParent(Managers.Keyword.PlayerKeywordPanel.transform);
        rectTransform.SetAsLastSibling();
        StartCoroutine(DragCheck());
        */
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
                SetMoveState(false);
            });
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == canvas)
        {
            //transform.SetParent(startParent);
            //rectTransform.position = startParent.GetComponent<RectTransform>().position;

            transform.DOLocalMove(startDragPoint, moveAnimDuration).SetEase(Ease.OutQuart);
        }
        else
        {
            transform.SetParent(startParent);
            //rectTransform.position = startParent.GetComponent<RectTransform>().position;
        }

        canvasGroup.blocksRaycasts = true;

        SetDragState(false);
        //transform.DOLocalMove(startDragPoint, moveAnimDuration).SetEase(Ease.OutQuart);

        Debug.Log(CheckOverlap());

        /*
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach(var result in results)
        {
            if(result.gameObject.CompareTag(KEYWORD_FRAME_TAG))
            {
                //들고 있는 오브젝트 드랍
                eventData.pointerDrag.transform.SetParent(transform);
            }
        }
        //var raycasts = Managers.Keyword.GetRaycastList(eventData);
        //Managers.Keyword.CurDragKeyword = null;
        */

        /*
        if (raycasts.Count > 0)
        {
            for (int i = 0; i < raycasts.Count; ++i)
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
#if UNITY_EDITOR
                else
                {
                    Debug.Log(raycasts[i].gameObject.name);
                }
#endif
            }
        }
        */
        //Managers.Sound.AskSfxPlay(20011);
        //ResetKeyword();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void DragReset()
    {
        transform.SetParent(startParent);
        transform.SetSiblingIndex(prevSibilintIndex);
        rectTransform.localPosition = startDragPoint;
        SetMoveState(false);
        SetDragState(false);
        curFrame.OnEndDrag();
    }

    public bool CheckOverlap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, LayerMask.GetMask("UI"));

        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider.gameObject.CompareTag(KEYWORD_FRAME_TAG))
            {
                return true;
            }
        }

        return false;
    }
}