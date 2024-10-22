using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMoveKeywordController : KeywordEntity
{
    private void Awake()
    {
        var attachOverride = new KeywordAction();
        attachOverride.OnFixecUpdate += OnAttachKeyword;
        AddOverrideTable(typeof(AttachKeyword).ToString(), attachOverride);
        var apartOverride = new KeywordAction();
        apartOverride.OnFixecUpdate += OnApartKeyword;
        AddOverrideTable(typeof(ApartKeyword).ToString(), apartOverride);
    }
    public void OnAttachKeyword(KeywordEntity entity)
    {
        KeywordEntity otherEntity;
        if (!PairKeyword.IsAvailablePair(entity, out otherEntity))
        {
            return;
        }

        var dir = otherEntity.KeywordTransformFactor.position - entity.KeywordTransformFactor.position;
        dir.y = 0;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            dir.z = 0;
        }
        else 
        {
            dir.x = 0;
        }
        if (dir.magnitude <= AttachKeyword.Speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER))
        {

            var isAble = entity.ColisionCheckMove(dir);
            entity.MoveAbleUpdate(isAble);
        }
        else
        {
            var isAble = entity.ColisionCheckMove(dir.normalized * AttachKeyword.Speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER));
            entity.MoveAbleUpdate(isAble);
        }
    }
    public void OnApartKeyword(KeywordEntity entity)
    {
        KeywordEntity otherEntity;
        if (!PairKeyword.IsAvailablePair(entity, out otherEntity))
        {
            return;
        }

        var dir = entity.KeywordTransformFactor.position - otherEntity.KeywordTransformFactor.position;
        dir.y = 0;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            dir.z = 0;
        }
        else
        {
            dir.x = 0;
        }

        var isAble = entity.ColisionCheckMove(dir.normalized * ApartKeyword.Speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER));
        entity.MoveAbleUpdate(isAble);
    }

}
