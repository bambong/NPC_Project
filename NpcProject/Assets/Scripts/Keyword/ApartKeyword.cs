using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartKeyword : KeywordController
{
    [SerializeField]
    public static float Speed = 10f;

    public override void OnEnter(KeywordEntity entity)
    {
        entity.WireColorController.AddColorState(WireColorStateController.E_WIRE_STATE.PAIR,E_WIRE_COLOR_MODE.Apart);
    }
    public override void OnFixedUpdate(KeywordEntity entity)
    {
        KeywordEntity otherEntity;
        if(!PairKeyword.IsAvailablePair(entity,out otherEntity)) 
        {
            return;
        }
        var dir = entity.KeywordTransformFactor.position -otherEntity.KeywordTransformFactor.position;
        dir.y = 0;
        //if(new Vector3(dir.x,0,dir.z).magnitude <= 0) 
        //{
        //    dir = Vector3.right;
        //}
        entity.ColisionCheckMove(dir.normalized * Speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER));
        
    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.WireColorController.RemoveColorState(WireColorStateController.E_WIRE_STATE.PAIR, E_WIRE_COLOR_MODE.Apart);
    }
}
