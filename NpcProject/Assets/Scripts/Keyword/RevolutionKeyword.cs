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
        KeywordEntity otherEntity;
        if (!PairKeyword.IsAvailablePair(entity, out otherEntity))  // 페어 키워드가 끼워진 다른 짝 오브젝트가 있는지 체크 하고 반환
        {
            return;
        }

        var orbitCenter = otherEntity.KeywordTransformFactor.position; // 짝 오브젝트의 중심을 저장
        var worldRotationAxis = Vector3.up; // 어떤 축을 중심으로 회전 시킬건지 Y축 사용 
        var dir = entity.KeywordTransformFactor.position - orbitCenter; 

        if (dir.magnitude > entity.RevAbleDistance)  // 영향을 받을 수 있는 최대 거리를 벗어 났는지 체크
        {
            return;
        }

        // 현재 프레임에서 얼만큼의 회전할 것인지 계산
        var newRotation = Quaternion.AngleAxis(Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER) * speed, worldRotationAxis);  
        var desiredOrbitPosition = orbitCenter + newRotation * dir; //구한 회전을 토대로 현재 프레임에 가야할 위치를 구함
        // 가야할 위치 - 현재 위치로 방향을 구하고 장애물을 체크하며 움직이는 함수를 호출
        entity.ColisionCheckMove((desiredOrbitPosition - entity.KeywordTransformFactor.position));

    }
    public override void OnRemove(KeywordEntity entity)
    {
        //entity.SetKinematic(false);
        entity.WireColorController.RemoveColorState(WireColorStateController.E_WIRE_STATE.PAIR, E_WIRE_COLOR_MODE.Revolution);
    }
}
