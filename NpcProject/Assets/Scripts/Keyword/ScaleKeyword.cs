using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScaleKeyword : KeywordController
{
    public override void KeywordAction(KeywordEntity entity)
    {
        var scale = entity.KeywordTransformFactor.localScale;
        scale *= 2;
        entity.KeywordTransformFactor.DOScale(scale,1);
    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.KeywordTransformFactor.DOScale(Vector3.one,0.5f);
    }
}
