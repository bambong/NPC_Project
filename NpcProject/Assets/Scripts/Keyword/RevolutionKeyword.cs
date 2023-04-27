using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionKeyword : KeywordController
{
    [SerializeField]
    private float speed = 10f;
    public override void OnEnter(KeywordEntity entity)
    {
        entity.WireColorController.AddColorState(WireColorStateController.E_WIRE_STATE.PAIR, E_WIRE_COLOR_MODE.Revolution);
    }
    public override void OnFixedUpdate(KeywordEntity entity)
    {
        //entity.ClearVelocity();
        //entity.SetKinematic(true);
        
        KeywordEntity otherEntity;
        if (!PairKeyword.IsAvailablePair(entity, out otherEntity))
        {
            return;
        }

        var _orbitCenter = otherEntity.KeywordTransformFactor.position;
        var _worldRotationAxis = Vector3.up;
        var dir = entity.KeywordTransformFactor.position - _orbitCenter;

        if (dir.magnitude > entity.RevAbleDistance) 
        {
            return;
        }

        // var _radius = dir.magnitude * Vector3.Normalize(dir);
        var _newRotation = Quaternion.AngleAxis(Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER) * speed,_worldRotationAxis);
        var _desiredOrbitPosition = _orbitCenter + _newRotation * dir;
        entity.ColisionCheckMove((_desiredOrbitPosition - entity.KeywordTransformFactor.position));

    }
    public override void OnRemove(KeywordEntity entity)
    {
        //entity.SetKinematic(false);
        entity.WireColorController.RemoveColorState(WireColorStateController.E_WIRE_STATE.PAIR, E_WIRE_COLOR_MODE.Revolution);
    }
}
