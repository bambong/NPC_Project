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
        // entity 에 등록된 키워드 중에 Pair 키워드가 있는지 체크
        foreach(var keyword in entity.CurrentRegisterKeyword)
        {
            if(keyword.Key is PairKeyword)
            {
                pairKeyword = keyword.Key as PairKeyword;
                break;
            }

        }
        // 페어 키워드가 없다면 반환
        if(pairKeyword == null)
        {
            return;
        }

        var target = pairKeyword.GetOtherPair().MasterEntity;
        // 다른 페어 키워드가 entity 에 들어가있는지 체크 
        if(target == null)
        {
            return;
        }


        var _orbitCenter = target.KeywordTransformFactor.position;
        var _worldRotationAxis = Vector3.up;
        var dir = entity.KeywordTransformFactor.position - _orbitCenter;

        // var _radius = dir.magnitude * Vector3.Normalize(dir);
        var _newRotation = Quaternion.AngleAxis(Managers.Time.GetDeltaTime(TIME_TYPE.PLAYER) * speed,_worldRotationAxis);
        var _desiredOrbitPosition = _orbitCenter + _newRotation * dir;
        entity.ColisionCheckMove((_desiredOrbitPosition - entity.KeywordTransformFactor.position));

    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.SetGravity(true);
    }
}
