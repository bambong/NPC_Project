using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private float constantAnimTime = 0.5f;
    public Transform KeywordSlotLayout { get => keywordSlotLayout.transform; }
    public float ConstantAnimTime { get => constantAnimTime; }

    private bool isDrag = false;
    private bool isKeywordSlotOpen;
    private KeywordEntity entity;


    public override void Init()
    {
        mask.sizeDelta = Vector2.zero;
    }
    public void RegisterEntity(KeywordEntity entity) 
    {
        this.entity = entity;
    }

    public void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(entity.transform.position);
    }
    public void DragOn() => isDrag = true;
    public void DragOff() 
    {
        isDrag = false;
        Close();
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
        float animTime = constantAnimTime * (1 - (mask.sizeDelta.x / size.x));
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

        Vector2 size = keywordSlotLayout.sizeDelta;
        size.x += diff.x;
        float animTime = constantAnimTime* (mask.sizeDelta.x / size.x);
        mask.DOSizeDelta(Vector2.zero, animTime);
    }
    private void OnDestroy()
    {
        mask.DOKill();
    }
  
}
