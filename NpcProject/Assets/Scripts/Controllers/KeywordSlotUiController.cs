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
    private Image debugIcon;
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
    private bool isFocus;
    private KeywordEntity entity;
    private readonly Vector3 FOCUS_DESIRE_SCALE = new Vector3(1.7f, 1.7f, 1);
    private const float FOCUS_ANIM_TIME = 0.5f;
    private const float CLOSE_ANIM_TIME = 0.3f;
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
        //Close();
    }

    public void FocusEnter()
    {
        if (!gameObject.activeInHierarchy) return;
        if (isFocus)
        {
            return;
        }
        isFocus = true;
        debugIcon.rectTransform.DOKill();
        float animTime = FOCUS_ANIM_TIME * (1 - (debugIcon.rectTransform.localScale.magnitude/FOCUS_DESIRE_SCALE.magnitude));
        debugIcon.rectTransform.DOScale(FOCUS_DESIRE_SCALE, animTime);
    }
    public void FocusExit()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!isFocus)
        {
            return;
        }
        isFocus = false;
        debugIcon.rectTransform.DOKill();
        float animTime = FOCUS_ANIM_TIME * (debugIcon.rectTransform.localScale.magnitude / FOCUS_DESIRE_SCALE.magnitude);
        debugIcon.rectTransform.DOScale(Vector3.one, animTime);
    }


    public void OpenKeywordPrepare()
    {
        if (!gameObject.activeInHierarchy) return;
        if (isPrepareImageOpen || isKeywordSlotOpen)
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
    public void ToggleOpen() 
    {
        if (isKeywordSlotOpen) 
        {
            Close();
        }
        else 
        {
            Open();
        }
    }
    public void Open()
    {
        if (!gameObject.activeInHierarchy) return;
        if (isKeywordSlotOpen)
        {
            return;
        }
        isKeywordSlotOpen = true;
        CloseKeywordPrepare();
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
        Vector2 size = keywordSlotLayout.sizeDelta;
        size.x += diff.x;
        size.y += diff.y;
        float animTime = CLOSE_ANIM_TIME * (mask.sizeDelta.magnitude / size.magnitude);
        mask.DOKill();
        mask.DOSizeDelta(Vector2.zero, animTime);
    }
 
    private void OnDestroy()
    {
        mask.DOKill();
    }

  
}
