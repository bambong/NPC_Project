using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachKeyword : KeywordController
{
    [SerializeField]
    public static float Speed = 10f;

    public override void OnEnter(KeywordEntity entity)
    {
        entity.WireColorController.AddColorState(WireColorStateController.E_WIRE_STATE.PAIR, E_WIRE_COLOR_MODE.Attach);

        KeywordEntity other;
        if (!PairKeyword.IsAvailablePair(entity, out other))
        {
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.MoveKeyword);
    }
    public override void OnFixedUpdate(KeywordEntity entity)
    {
        KeywordEntity otherEntity;
        if (!PairKeyword.IsAvailablePair(entity, out otherEntity))
        {
            return;
        }       
        var dir = otherEntity.KeywordTransformFactor.position - entity.KeywordTransformFactor.position;
        dir.y = 0;
        if(dir.magnitude <= Speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER)) 
        {
            entity.ColisionCheckMove(dir);
        }
        else 
        {
            entity.ColisionCheckMove(dir.normalized * Speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER));
        }
    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.WireColorController.RemoveColorState(WireColorStateController.E_WIRE_STATE.PAIR, E_WIRE_COLOR_MODE.Attach);
        isPlay = true;
    }
}
