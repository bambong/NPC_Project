using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScaleKeyword : KeywordController
{
    private Vector3 prevScale;
    public override void KeywordAction(KeywordEntity entity)
    {
        var scale = entity.transform.localScale;
        prevScale = scale;
        scale *= 2;
        entity.transform.localScale = scale;
    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.transform.DOScale(prevScale,1);
    }
}
