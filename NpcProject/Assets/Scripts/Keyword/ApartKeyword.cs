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
        var dir = entity.KeywordTransformFactor.position -target.KeywordTransformFactor.position;
        dir.y = 0;
        if(new Vector3(dir.x,0,dir.z).magnitude <= 0) 
        {
            dir = Vector3.right;
        }
        entity.ColisionCheckMove(dir.normalized * speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.PLAYER));
        
    }
    public override void OnRemove(KeywordEntity entity)
    {
       entity.SetKinematic(false);
    }
}
