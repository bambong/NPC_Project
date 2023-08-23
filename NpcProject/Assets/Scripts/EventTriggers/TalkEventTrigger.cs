
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TalkEventTrigger : EventTrigger
{
    [SerializeField]
    private int talk;
    [SerializeField]
    private UnityEvent startEvent;
    [SerializeField]
    private UnityEvent endEvent;

    public override void OnEventTrigger(Collider other)
    {
        base.OnEventTrigger(other);
        var dialogue = Managers.Talk.GetTalkEvent(talk);
        dialogue.OnStart(() => startEvent?.Invoke());
        dialogue.OnComplete(() => endEvent?.Invoke());
        Managers.Talk.PlayCurrentSceneTalk(talk);
    }
}
