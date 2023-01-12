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
   
    private bool hasKeyword =false;
    private KeywordController keywordController;
    public bool HasKeyword { get => hasKeyword; }
    public KeywordController KeywordController { get => keywordController; }

    public bool SetKeyWord(KeywordController keywordController) 
    {
        if (hasKeyword) 
        {
            return false;
        }
        hasKeyword = true;
        this.keywordController = keywordController;
        keywordController.EnterKeywordAction(Managers.Keyword.CurKeywordEntity);
        keywordController.transform.parent = transform;
        keywordController.SetToKeywordFrame(rectTransform.position);
        return true;
    }

 
    public void ResetKeywordFrame() 
    {
        if(keywordController == null) 
        {
            return;
        }
        keywordController.ExitKeywordAction(Managers.Keyword.CurKeywordEntity);
        hasKeyword = false;
        keywordController = null;
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
