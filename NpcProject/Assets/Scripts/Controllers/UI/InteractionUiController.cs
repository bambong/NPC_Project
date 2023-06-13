using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class InteractionUiController : UI_Base
{
    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private Vector2 sizeDelta;
    private Vector2 idleSizeDelta;
    private float sizeDeltaDiff;

    private const float Y_POS_REVISION_AMOUNT = 3;
    private const float OPEN_ANIM_TIME = 0.5f;
    private const float CLOSE_ANIM_TIME = 0.5f;
    //private bool isOpen = false;
    private const float IDLE_SIZE_AMOUNT = 1.1f;
    private const float IDLE_ANIM_TIME = 0.3f;

    private void IdleAnim() 
    {
        var diff =  (rectTransform.sizeDelta.x - sizeDelta.x) / sizeDeltaDiff; 
        rectTransform.DOKill();
        rectTransform.DOSizeDelta(sizeDelta, diff * IDLE_ANIM_TIME).OnComplete(()=>
        {
            rectTransform.sizeDelta = sizeDelta;
            rectTransform.DOSizeDelta(idleSizeDelta, IDLE_ANIM_TIME).SetLoops(-1, LoopType.Yoyo);
        });
    }

    public void Open()
    {
        gameObject.SetActive(true);
        canvasGroup.DOKill();
        IdleAnim();
        var animTime = OPEN_ANIM_TIME * (1 - canvasGroup.alpha);
        canvasGroup.DOFade(1, animTime);
    }
    public void Open(Transform parent)
    {
        transform.position = parent.position + Vector3.up * ((parent.GetComponent<Collider>().bounds.size.y/2)+ Y_POS_REVISION_AMOUNT);
        transform.rotation  = Camera.main.transform.rotation;
        transform.SetParent(parent);
       
        gameObject.SetActive(true);
        IdleAnim();
        canvasGroup.DOKill();
        var animTime = OPEN_ANIM_TIME * (1 - canvasGroup.alpha);
        canvasGroup.DOFade(1, animTime);
    }
    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
    public void Close()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        canvasGroup.DOKill();
        var animTime = CLOSE_ANIM_TIME * canvasGroup.alpha;
        canvasGroup.DOFade(0, animTime).OnComplete(() => {
            rectTransform.sizeDelta = sizeDelta;
            rectTransform.DOKill();
            gameObject.SetActive(false); });
    } 

    public override void Init()
    {
        sizeDelta = rectTransform.sizeDelta;
        idleSizeDelta = rectTransform.sizeDelta * IDLE_SIZE_AMOUNT;
        sizeDeltaDiff = sizeDelta.x - idleSizeDelta.x;
    }
}
