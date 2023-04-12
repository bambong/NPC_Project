using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordFrameController : KeywordFrameBase
{
    [SerializeField]
    private Image raycastImage;


    private KeywordController registerKeyword;

   private KeywordWorldSlotUIController keywordWorldSlot;

    private KeywordEntity entity;
    public bool IsKeywordRemoved { get { return (registerKeyword != null && curFrameInnerKeyword != registerKeyword); } }

    public KeywordController RegisterKeyword { get => registerKeyword; }
    public KeywordWorldSlotUIController KeywordWorldSlot { get => keywordWorldSlot;  }

    protected override void DecisionKeyword()
    {
        entity.DecisionKeyword(this);
    }
    public void SetLockFrame(bool isOn) 
    {
        Color frameColor = Color.black;
        if (isOn) 
        {
            frameColor = new Color(0.4f, 0.4f, 0.4f);
        }

        SetFrameColor(frameColor);
        raycastImage.raycastTarget = !isOn;
        curFrameInnerKeyword.SetLockState(isOn);
    }
    public void OnDecisionKeyword() 
    {
        registerKeyword = curFrameInnerKeyword;
    }

    public void RegisterEntity(KeywordEntity entity ,KeywordWorldSlotUIController keywordWorldSlot)
    {
        this.entity = entity;
        this.keywordWorldSlot = keywordWorldSlot;
    }
    public override void ResetKeywordFrame()
    {
        base.ResetKeywordFrame();
        registerKeyword = null;
    }
    public override void OnBeginDrag()
    {
        entity.KeywordSlotUiController.DragOn();
    }
    public override void OnEndDrag()
    {
        entity.KeywordSlotUiController.DragOff();
        entity.DecisionKeyword(this);
    }
    public override void Init()
    {
        ResetKeywordFrame();
    }
    private void ClearLock() 
    {
        SetFrameColor(Color.black);
        raycastImage.raycastTarget = true;
    }
    public void ClearForPool()
    {
        if(curFrameInnerKeyword != null) 
        {
            curFrameInnerKeyword.DestroyKeyword();
        }
        ClearDotween();
        ClearLock();
        Managers.Resource.Destroy(transform.parent.gameObject);
    }
}
