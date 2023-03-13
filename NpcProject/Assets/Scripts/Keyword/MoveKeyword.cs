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
    public override void OnFixedUpdate(KeywordEntity entity)
    {
        if(!entity.ColisionCheckMove( moveVector *dir* moveSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER))) 
        {
            Turn();
        }
    }

    
}
