using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeywordFrameBase : UI_Base
{
    public virtual bool IsAvailable { get => true; }
    public abstract void SetKeyWord(KeywordController keywordController);
    public virtual void ResetKeywordFrame(){}
    public override void Init(){}
}
