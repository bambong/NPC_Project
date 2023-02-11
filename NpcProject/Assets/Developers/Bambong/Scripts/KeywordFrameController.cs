using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordFrameController : UI_Base
{
    [SerializeField]
    private GameObject parentObj;
    [SerializeField]
    private RectTransform rectTransform;

    private KeywordController registerKeyword;
    private KeywordController curFrameInnerKeyword;
    public bool HasKeyword { get => CurFrameInnerKeyword != null; }
    public bool IsKeywordRemoved { get { return (registerKeyword != null && curFrameInnerKeyword != registerKeyword); } }

    public KeywordController CurFrameInnerKeyword { get => curFrameInnerKeyword; }
    public KeywordController RegisterKeyword { get => registerKeyword;  }

    public bool SetKeyWord(KeywordController keywordController) 
    {
        this.curFrameInnerKeyword = keywordController;
        keywordController.transform.parent = transform;
        keywordController.SetToKeywordFrame(rectTransform.position);
        return true;
    }
    public void OnDecisionKeyword() 
    {
        registerKeyword = curFrameInnerKeyword;
    }

    public void ResetKeywordFrame() 
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
