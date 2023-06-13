using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PairPuzzleEntityController : KeywordEntity
{
    [SerializeField]
    private UnityEvent onClear;

    public override void AddAction(KeywordController controller, KeywordAction action)
    {
        base.AddAction(controller, action);
        if(controller is PairKeyword)
        {
            var pair = controller as PairKeyword;
            if (pair.GetOtherPair().MasterEntity != null) 
            {
                onClear?.Invoke();
            }
        }
    }
}
