using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class KeywordSlotUiController : UI_Base
{
    [SerializeField]
    private RectTransform keywordSlotLayout;
    [SerializeField]
    private RectTransform mask;
    [SerializeField]
    private Vector2 diff;
    [SerializeField]
    private Image prepareImage;
    [SerializeField]
    private CanvasGroup prepareCanvasGroup;
    [SerializeField]
    private Vector3 prepareDesireSize;

    [SerializeField]
    private float constantAnimTime = 0.5f;
    public Transform KeywordSlotLayout { get => keywordSlotLayout.transform; }
 
    private bool isDrag = false;
    private bool isKeywordSlotOpen;
    private bool isPrepareImageOpen;
    private KeywordEntity entity;


    public override void Init()
    {
        mask.sizeDelta = Vector2.zero;
        //prepareImage.rectTransform.sizeDelta = Vector2.zero;
        prepareCanvasGroup.alpha = 0;
    }
    public void RegisterEntity(KeywordEntity entity) 
    {
        this.entity = entity;
    }

    public void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(entity.transform.position);
    }
    public void DragOn()
    {

    //    isDrag = true;
    }
    public void DragOff() 
    {
      //  isDrag = false;
        Close();
    }

    public void OpenKeywordPrepare()
    {
        if (!gameObject.activeInHierarchy) return;
        if (isPrepareImageOpen)
        {
            return;
        }
        isPrepareImageOpen = true;
        prepareCanvasGroup.DOKill();
        float animTime = constantAnimTime * (1 - prepareCanvasGroup.alpha);
        prepareCanvasGroup.DOFade(1, animTime);
    }

    public void CloseKeywordPrepare()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!isPrepareImageOpen)
        {
            return;
        }
        isPrepareImageOpen = false;
        prepareCanvasGroup.DOKill();
        float animTime = constantAnimTime * prepareCanvasGroup.alpha;
        prepareCanvasGroup.DOFade(0, animTime);
    }

    public void Open()
    {
        if (!gameObject.activeInHierarchy) return;
        if (isKeywordSlotOpen)
        {
            return;
        }
        isKeywordSlotOpen = true;
        transform.SetAsLastSibling();
        mask.DOKill();
        Vector2 size = keywordSlotLayout.sizeDelta;
        size.x += diff.x;
        size.y += diff.y;
        float animTime = constantAnimTime * (1 - (mask.sizeDelta.magnitude / size.magnitude));
        mask.DOSizeDelta(size, animTime);
    }
    public void Close()
    {
        if (!gameObject.activeInHierarchy) return;

        if (isDrag || !isKeywordSlotOpen)
        {
            return;
        }
        isKeywordSlotOpen = false;
        mask.DOKill();
        mask.DOSizeDelta(Vector2.zero, constantAnimTime);
    }
    public bool IsMouseInPrepare()
    {
        Vector3 mousePos = Input.mousePosition;
        // 이미지 RectTransform을 가져옴
        RectTransform imageRect = prepareImage.rectTransform;

        // 이미지 내부에 있는지 확인
        if (RectTransformUtility.RectangleContainsScreenPoint(imageRect, mousePos))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    
    private void OnDestroy()
    {
        mask.DOKill();
    }

  
}
