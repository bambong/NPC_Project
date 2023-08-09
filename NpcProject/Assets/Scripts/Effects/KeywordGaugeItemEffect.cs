using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordGaugeItemEffect : GameEffect
{
    public override void Play(Transform targetTrs)
    {
        transform.position = targetTrs.transform.position;
        base.Play(targetTrs);
    }
}
