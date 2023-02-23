using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeywordFrame : KeywordFrameBase
{
    public override void SetKeyWord(KeywordController keywordController)
    {
        Managers.Keyword.AddKeywordToDebugZone(Managers.Keyword.CurDebugZone,keywordController);
        
      
    }
}
