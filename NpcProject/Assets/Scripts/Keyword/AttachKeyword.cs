using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachKeyword : KeywordController
{
    [SerializeField]
    private float speed = 10f; 
    public override void KeywordAction(KeywordEntity entity)
    {

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
        var dir = target.transform.position - entity.transform.position;
        entity.ColisionCheckMove(dir.normalized * speed);
    }
    public override void OnRemove(KeywordEntity entity)
    {
        
    }
}
