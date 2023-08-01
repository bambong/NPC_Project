using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeywordLayoutController : MonoBehaviour
{

    [SerializeField]
    RectTransform rectTransform;

    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    CanvasGroup noticeCanvasGroup;

    [SerializeField]
    private float moveTime = 1f;
    [SerializeField]
    private Vector3 desirePos;
    [SerializeField]
    private Vector3 startPos;

    private bool isOpen;

    public bool IsOpen { get => isOpen;}

    void Start()
    {
        rectTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 0;
        noticeCanvasGroup.alpha = 1;
    }

    public void Open() 
    {
        if (isOpen)
        {
            return;
        }
        isOpen = true;
        rectTransform.DOKill();
        canvasGroup.DOKill();
        noticeCanvasGroup.DOKill();
        float curTime = moveTime * (1 - canvasGroup.alpha);
        canvasGroup.DOFade(1, curTime);
        noticeCanvasGroup.DOFade(0, curTime);
        rectTransform.DOAnchorPos(desirePos, curTime);
    }
  
    public void Close()
    {
        if (!isOpen)
        {
            return;
        }
        isOpen = false;
        rectTransform.DOKill();
        canvasGroup.DOKill();
        noticeCanvasGroup.DOKill();
        float curTime = moveTime * canvasGroup.alpha;
        canvasGroup.DOFade(0, curTime);
        noticeCanvasGroup.DOFade(1, curTime);
        rectTransform.DOAnchorPos(startPos, curTime);
    }
}
