using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveKeyword : KeywordController
{
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private Vector3 moveVector;
    private int dir = 1;
    private void Turn() 
    {
        dir *= -1;
    }
    public override void KeywordUpdateAction(KeywordEntity entity)
    {
        if(!entity.ColisionCheckMove( moveVector *dir* moveSpeed)) 
        {
            Turn();
        }
    }
    public override void EnterKeywordAction(KeywordEntity entity)
    {
        dir = 1;
        //entity.transform.DORotate(new Vector3(0,360,0),2f).SetLoops(-1,LoopType.Incremental).SetEase(Ease.Linear);
    }
    public override void ExitKeywordAction(KeywordEntity entity)
    {
        dir = 1;
        //entity.transform.DOKill();
    }
    
}
