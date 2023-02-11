using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairKeyword : KeywordController
{
    public static List<PairKeyword> PairKeywords;

    public KeywordEntity MasterEntity { get; protected set; }

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
    private void Awake()
    {
        PairKeywords.Add(this);
    }

    public override void KeywordAction(KeywordEntity entity)
    {
        MasterEntity = entity;
    }
    public override void OnRemove(KeywordEntity entity)
    {
        MasterEntity = null;
    }

    private void OnDestroy()
    {
        PairKeywords.Remove(this);
    }
}
