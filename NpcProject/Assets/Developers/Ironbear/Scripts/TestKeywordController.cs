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
    private Canvas canvas;

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

    public KeywordFrameBase CurFrame { get => curFrame; }
    public RectTransform RectTransform { get => rectTransform; }

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
        canvasGroup.blocksRaycasts = false;
        startParent = transform.parent;
        transform.SetParent(GameObject.FindGameObjectWithTag("UI Canvas").transform);

        prevSibilintIndex = rectTransform.GetSiblingIndex();
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
        canvasGroup.blocksRaycasts = true;

        SetDragState(false);
        transform.SetParent(startParent);
        //transform.localPosition = startDragPoint;
        transform.DOLocalMove(startDragPoint, moveAnimDuration).SetEase(Ease.OutQuart);
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

    IEnumerator DragCheck()
    {
        while (isDrag)
        {
            if (!Input.GetMouseButton(0))
            {
                Managers.Sound.AskSfxPlay(20011);
                DragReset();
                Debug.Log("유니티 드래그 버그 발생!");
                yield break;
            }
            yield return null;
        }
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
}