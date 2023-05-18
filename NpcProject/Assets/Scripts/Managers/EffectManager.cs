using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager 
{
    public void PlayEffect(Define.EFFECT effectName,Transform targetTrs) 
    {
        var effect = Managers.Resource.Instantiate("Effects/" + effectName.ToString()).GetComponent<GameEffect>();
        effect.Play(targetTrs);
    }
}
