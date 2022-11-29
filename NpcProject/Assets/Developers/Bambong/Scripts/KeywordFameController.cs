using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordFameController : MonoBehaviour
{
    [SerializeField]
    private E_KeywordType keywordType;

    [SerializeField]
    private RectTransform rectTransform;

    private bool hasKeyword =false;
    private KeywordController keywordController;
   
    public bool HasKeyword { get => hasKeyword; }
    public KeywordController KeywordController { get => keywordController; }

    public bool SetKeyWord(KeywordController keywordController) 
    {
        if (hasKeyword || !keywordController.CompareKeywordType(keywordType)) 
        {
            return false;
        }
        hasKeyword = true;
        this.keywordController = keywordController;
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
        keywordController.ResetKeyword();
        keywordController = null;
    }
}
