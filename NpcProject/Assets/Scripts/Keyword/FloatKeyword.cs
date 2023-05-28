using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatKeyword : KeywordController
{
    [SerializeField]
    private float speed = 10;

    public override void OnEnter(KeywordEntity entity)
    {
        if (isPlay)
        {
            Managers.Sound.PlaySFX(Define.SOUND.FloatingKeyword);
        }
        entity.WireColorController.AddColorState(WireColorStateController.E_WIRE_STATE.NORMAL, E_WIRE_COLOR_MODE.Float);
        isPlay = true;
    }
    public override void OnFixedUpdate(KeywordEntity entity)
    {
        entity.SetGravity(false);
        entity.SetKinematic(true);

        entity.FloatMove(Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER) * speed);
    

    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.SetGravity(true);
        entity.SetKinematic(false);
        entity.WireColorController.RemoveColorState(WireColorStateController.E_WIRE_STATE.NORMAL, E_WIRE_COLOR_MODE.Float);

    }

}
