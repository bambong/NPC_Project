using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;



public class KeywordManager : GameObjectSingleton<KeywordManager>, IInit
{
    [SerializeField]
    private KeywordFameController objectKeywordFrame;
    
    [SerializeField]
    private KeywordFameController actionKeywordFrame;

    public void Init()
    {
    }

    public void SetKeyWord(KeywordController keywordController)
    {
        if (objectKeywordFrame.SetKeyWord(keywordController)) 
        {
            Interaction();    
            return;
        }

        if (actionKeywordFrame.SetKeyWord(keywordController)) 
        {
            Interaction();    
            return;
        }

        keywordController.ResetKeyword();
        
    }

    private void Interaction() 
    {
        if(!objectKeywordFrame.HasKeyword || !actionKeywordFrame.HasKeyword) 
        {
            return;
        }

        if (!actionKeywordFrame.InteractionKeyword(objectKeywordFrame.KeywordController)) 
        {
            ResetKeywordFrame();
        }
        

    }
    public void ResetKeywordFrame() 
    {
        actionKeywordFrame.ResetKeywordFrame();
        objectKeywordFrame.ResetKeywordFrame();
    }



}
