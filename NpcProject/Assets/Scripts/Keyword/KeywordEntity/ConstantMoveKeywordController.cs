using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMoveKeywordController : KeywordEntity
{
    private void Awake()
    {

        AddOverrideTable(typeof(AttachKeyword).ToString(), new KeywordAction(OnAttachKeyword, KeywordActionType.OnUpdate, null));
        AddOverrideTable(typeof(ApartKeyword).ToString(), new KeywordAction(OnApartKeyword, KeywordActionType.OnUpdate, null));
    }
    public void OnAttachKeyword(KeywordEntity entity)
    {
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
        var dir = target.KeywordTransformFactor.position - entity.KeywordTransformFactor.position;
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
            entity.ColisionCheckMove(dir);
        }
        else 
        {
            entity.ColisionCheckMove(dir.normalized * AttachKeyword.Speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER));
        }
    }
    public void OnApartKeyword(KeywordEntity entity)
    {
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
        var dir = entity.KeywordTransformFactor.position - target.KeywordTransformFactor.position;
        dir.y = 0;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            dir.z = 0;
        }
        else
        {
            dir.x = 0;
        }
        if (new Vector3(dir.x, 0, dir.z).magnitude <= 0)
        {
            dir = Vector3.right;
        }
        entity.ColisionCheckMove(dir.normalized * ApartKeyword.Speed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER));
    }
    //public void OnScaleKeyword(KeywordEntity entity) 
    //{
    //    if (transform.lossyScale.magnitude < scaleDesire.magnitude)
    //    {
    //        //speed = InSine(transform.lossyScale.magnitude / targetScale.magnitude)*10;
    //        var curFrameDesirScale = transform.lossyScale + (originScale * Time.deltaTime * 10);
    //        if (curFrameDesirScale.magnitude > scaleDesire.magnitude)
    //        {
    //            curFrameDesirScale = scaleDesire;
    //        }
    //        entity.ColisionCheckScale(scaleDesire,ScaleKeyword)
    //    }
    //    //onScaleFeedback.PlayFeedbacks();
    //}
}
