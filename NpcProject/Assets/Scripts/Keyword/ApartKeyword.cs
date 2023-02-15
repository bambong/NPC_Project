using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartKeyword : KeywordController
{
    [SerializeField]
    private float speed = 10f; 
    public override void KeywordAction(KeywordEntity entity)
    {
        entity.ClearVelocity();
        entity.SetGravity(false);
        PairKeyword pairKeyword = null;
        foreach(var keyword in entity.CurrentRegisterKeyword)
        {
            if(keyword.Key is PairKeyword) 
            {
                pairKeyword = keyword.Key as PairKeyword;
                break;            
            }
            
        }
        if(pairKeyword == null) 
        {
            return;
        }
        var target = pairKeyword.GetOtherPair().MasterEntity;

        if(target == null)
        {
            return;
        }
        var dir = entity.KeywordTransformFactor.position -target.KeywordTransformFactor.position;
        dir.y = 0;

        entity.ColisionCheckMove(dir.normalized * speed);
        
    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.SetGravity(true);
    }
}
