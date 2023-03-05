using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairKeyword : KeywordController
{
    public static Dictionary<DebugZone, List<PairKeyword>> PairKeywords = new Dictionary<DebugZone,List<PairKeyword>>();

    public KeywordEntity MasterEntity { get; protected set; }

    public PairKeyword GetOtherPair() 
    {
        for(int i = 0;i < PairKeywords[parentDebugZone].Count; ++i) 
        {
            if (PairKeywords[parentDebugZone][i] == this) 
            {
                continue;
            }
            return PairKeywords[parentDebugZone][i];
        }
        return null;
    }
    public override void SetDebugZone(DebugZone zone)
    {
        base.SetDebugZone(zone);
        if(!PairKeywords.ContainsKey(zone)) 
        {
            PairKeywords.Add(zone, new List<PairKeyword>());
        }
        PairKeywords[zone].Add(this);
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
        PairKeywords[parentDebugZone].Remove(this);
        if(PairKeywords[parentDebugZone].Count == 0) 
        {
            PairKeywords.Remove(parentDebugZone);
        }
    }
}
