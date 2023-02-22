using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScaleKeyword : KeywordController
{
    [SerializeField]
    private float speed =10;
    private GameObject parentTemp;
    public override void Init()
    {
        parentTemp = new GameObject();
        parentTemp.hideFlags = HideFlags.HideInHierarchy;
    }

    public override void KeywordAction(KeywordEntity entity)
    {
        if (entity.transform.lossyScale.magnitude < entity.MaxScale.magnitude)
        {
            var curFrameDesirScale = entity.transform.lossyScale + (entity.OriginScale * Time.deltaTime * speed);
            if (curFrameDesirScale.magnitude > entity.MaxScale.magnitude)
            {
                curFrameDesirScale = entity.MaxScale;
            }
            entity.ColisionCheckScale(curFrameDesirScale, parentTemp);
        }
    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.KeywordTransformFactor.DOScale(Vector3.one,0.5f);
    }
}
