using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class KeywordFrameController : KeywordFrameBase
{
    [SerializeField]
    private Image raycastImage;

  

     private KeywordStatusUiController keywordWorldSlot;
    
    public KeywordStatusUiController KeywordWorldSlot { get => keywordWorldSlot;  }
   
    protected override void DecisionKeyword(KeywordController keyword)
    {
        masterEntity.DecisionKeyword(this, keyword);
    }
    public void SetLockFrame(bool isOn) 
    {
        Color frameColor = Color.black;
        if (isOn) 
        {
            frameColor = new Color(0.4f, 0.4f, 0.4f);
        }
        isLock = isOn;

        SetFrameColor(frameColor);
        raycastImage.raycastTarget = !isOn;
        curFrameInnerKeyword.SetLockState(isOn);
    }

    public void RegisterEntity(KeywordEntity entity ,KeywordStatusUiController keywordWorldSlot)
    {
        this.masterEntity = entity;
        this.keywordWorldSlot = keywordWorldSlot;
    }
    public override void ResetKeywordFrame()
    {
        base.ResetKeywordFrame();
    }
    public override void OnBeginDrag()
    {
        masterEntity.KeywordSlotUiController.DragOn();
    }
    public override void OnEndDrag()
    {
        masterEntity.KeywordSlotUiController.DragOff();
    }
    public override void Init()
    {
        ResetKeywordFrame();
    }
    private void ClearLock() 
    {
        SetFrameColor(Color.black);
        isLock = false;
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
