using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionKeyword : KeywordController
{
    [SerializeField]
    private float speed = 10f;
    public override void KeywordAction(KeywordEntity entity)
    {
        entity.ClearVelocity();
        entity.SetGravity(false);
        PairKeyword pairKeyword = null;
        foreach (var keyword in entity.CurrentRegisterKeyword)
        {
            if (keyword.Key is PairKeyword)
            {
                pairKeyword = keyword.Key as PairKeyword;
                break;
            }

        }
        if (pairKeyword == null)
        {
            return;
        }
        var target = pairKeyword.GetOtherPair().MasterEntity;

        if (target == null)
        {
            return;
        }


        var _orbitCenter = target.KeywordTransformFactor.position;
        var _worldRotationAxis =  Vector3.up;
        var dir = entity.KeywordTransformFactor.position - _orbitCenter;

       // var _radius = dir.magnitude * Vector3.Normalize(dir);
        var _newRotation = Quaternion.AngleAxis(Managers.Time.GetDeltaTime(TIME_TYPE.PLAYER)*speed, _worldRotationAxis);
        var _desiredOrbitPosition = _orbitCenter + _newRotation * dir;
        entity.ColisionCheckMove((_desiredOrbitPosition - entity.KeywordTransformFactor.position));

    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.SetGravity(true);
    }
}
