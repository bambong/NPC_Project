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


    private readonly float OPEN_ANIM_TIME = 0.3f;
    private readonly float ClOSE_ANIM_TIME = 0.3f;
    public Transform KeywordSlotLayout { get => keywordSlotLayout.transform; }
 
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
        float animTime = OPEN_ANIM_TIME * (1 - (mask.sizeDelta.magnitude / size.magnitude));
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
        mask.DOSizeDelta(Vector2.zero, ClOSE_ANIM_TIME);
    }
    private void OnDestroy()
    {
        mask.DOKill();
    }
  
}
