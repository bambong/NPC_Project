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

    [SerializeField]
    private Image[] frameColorImages;

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
      
        for(int i =0; i< frameColorImages.Length; ++i) 
        {
            frameColorImages[i].color = frameColor;
        }
        raycastImage.raycastTarget = !isOn;
        curFrameInnerKeyword.SetLock(isOn);
    }
    public void OnDecisionKeyword() 
    {
        registerKeyword = curFrameInnerKeyword;
    }

    public override void ResetKeywordFrame() 
    {
        curFrameInnerKeyword = null;
    }
    public void RegisterEntity(KeywordEntity entity ,KeywordWorldSlotUIController keywordWorldSlot)
    {
        this.entity = entity;
        this.keywordWorldSlot = keywordWorldSlot;
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
        
    }
}
