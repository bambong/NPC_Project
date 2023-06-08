
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkEventTrigger : EventTrigger
{
    [SerializeField]
    private int talk;

    public override void OnEventTrigger(Collider other)
    {
        base.OnEventTrigger(other);
        Managers.Talk.PlayCurrentSceneTalk(talk);

    }
}
