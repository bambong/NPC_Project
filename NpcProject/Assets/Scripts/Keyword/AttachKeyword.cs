using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachKeyword : KeywordController
{
    [SerializeField]
    private float speed = 10f; 
    public override void KeywordAction(KeywordEntity entity)
    {
        entity.ClearVelocity();
        entity.SetKinematic(true);
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
        var dir = target.KeywordTransformFactor.position - entity.KeywordTransformFactor.position;
        dir.y = 0;
        if(dir.magnitude < speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.PLAYER)) 
        {
            entity.ColisionCheckMove(dir);
        }
        else 
        {
            entity.ColisionCheckMove(dir.normalized * speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.PLAYER));
        }
    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.SetKinematic(false);
    }
}
