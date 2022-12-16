using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordFameController : UI_Base
{
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
        keywordController.ClearCurFrame();
        this.keywordController = keywordController;
        keywordController.transform.parent = transform;
        keywordController.SetToKeywordFrame(rectTransform.position);
        return true;
    }

    public bool InteractionKeyword(KeywordController keywordController)
    {
        return this.keywordController.HandleObjectKeyword(keywordController);
    }
    public void ResetKeywordFrame() 
    {
        hasKeyword = false;
        keywordController = null;
    }
    public void Open() 
    {
        gameObject.SetActive(true);
    }
    public void Close() 
    {
        gameObject.SetActive(false);
    }

    public override void Init()
    {
        
    }
}
