using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeywordFrame : KeywordFrameBase
{
    public override void SetKeyWord(KeywordController keywordController, Action onComplete = null)
    {
        base.SetKeyWord(keywordController, onComplete);
        if (keywordController is PairKeyword)
        {
            keywordController.OnRemove(null); ;
        }
    }
}
