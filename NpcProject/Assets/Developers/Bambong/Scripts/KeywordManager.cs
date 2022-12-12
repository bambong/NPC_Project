using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class KeywordManager
{
    [SerializeField]
    private KeywordFameController objectKeywordFrame;
    
    [SerializeField]
    private KeywordFameController actionKeywordFrame;

    [SerializeField]
    private List<GridLayoutGroup> gridLayoutGroups;

    private Dictionary<string, KeywordEntity> keywords = new Dictionary<string, KeywordEntity>();


    public void Init()
    {
    }

    public void SetKeyWord(KeywordController keywordController)
    {
        if (objectKeywordFrame.SetKeyWord(keywordController)) 
        {
            return;
        }

        if (actionKeywordFrame.SetKeyWord(keywordController)) 
        {
            return;
        }

        keywordController.ResetKeyword();
        
    }

    public void Interaction() 
    {
        if(!objectKeywordFrame.HasKeyword || !actionKeywordFrame.HasKeyword) 
        {
            return;
        }

        if (!actionKeywordFrame.InteractionKeyword(objectKeywordFrame.KeywordController)) 
        {
            ResetKeyword();
            ResetKeywordFrame();
        }

    }
    public void ResetKeywordFrame() 
    {
        actionKeywordFrame.ResetKeywordFrame();
        objectKeywordFrame.ResetKeywordFrame();
    }
    public void ResetKeyword() 
    {
        actionKeywordFrame.ResetKeyword();
        objectKeywordFrame.ResetKeyword();
    }
    public void ResortKeywordArea() 
    {
        foreach(var grid in gridLayoutGroups) 
        {
            grid.enabled = true;
            grid.enabled = false;
        }
    }

}
