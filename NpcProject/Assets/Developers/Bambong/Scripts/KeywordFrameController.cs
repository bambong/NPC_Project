using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordFrameController : KeywordFrameBase
{
    [SerializeField]
    private GameObject parentObj;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image raycastImage;

    [SerializeField]
    private Image[] frameColorImages;

    private KeywordController registerKeyword;
    private KeywordController curFrameInnerKeyword;
    public bool HasKeyword { get => CurFrameInnerKeyword != null; }
    public bool IsKeywordRemoved { get { return (registerKeyword != null && curFrameInnerKeyword != registerKeyword); } }

    public KeywordController CurFrameInnerKeyword { get => curFrameInnerKeyword; }
    public KeywordController RegisterKeyword { get => registerKeyword; }


    public override bool IsAvailable{ get => curFrameInnerKeyword == null; }
    public override void SetKeyWord(KeywordController keywordController) 
    {
        curFrameInnerKeyword = keywordController;
        keywordController.transform.SetParent(transform);
        keywordController.SetToKeywordFrame(rectTransform.position);
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
 
    public void Open() 
    {
        parentObj.SetActive(true);
    }
    public void Close() 
    {
        parentObj.SetActive(false);
    }
    public void SetScale(Vector3 scale) 
    {
        parentObj.transform.localScale = scale;
    }

    public override void Init()
    {
        
    }
}
