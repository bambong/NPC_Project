using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairKeyword : KeywordController
{
    public static List<PairKeyword> PairKeywords;
    
    public PairKeyword GetOtherPair() 
    {
        for(int i = 0;i < PairKeywords.Count; ++i) 
        {
            if(PairKeywords[i] == this) 
            {
                continue;
            }
            return PairKeywords[i];
        }
        return null;
    }

    public override void KeywordAction(KeywordEntity entity)
    {
    }
}
