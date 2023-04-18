using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordSlotUiController : UI_Base
{
    [SerializeField]
    private RectTransform keywordSlotLayout;
    [SerializeField]
    private RectTransform mask;

    private KeywordEntity entity;
    public Transform KeywordSlotLayout { get => keywordSlotLayout.transform; }
    private readonly float OPEN_ANIM_TIME = 0.3f;
    private readonly float ClOSE_ANIM_TIME = 0.3f;
    private bool isDrag = false;
    private bool isKeywordSlotOpen;

    public override void Init()
    {
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

        if (!Managers.Game.IsDebugMod)
        {
            Close();
            return;
        }
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
        float animTime = OPEN_ANIM_TIME * (1 - (mask.sizeDelta.magnitude / keywordSlotLayout.sizeDelta.magnitude));
        mask.DOSizeDelta(keywordSlotLayout.sizeDelta, animTime);
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
