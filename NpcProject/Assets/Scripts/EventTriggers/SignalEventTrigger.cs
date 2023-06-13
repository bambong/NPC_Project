using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalEventTrigger : EventTrigger
{
    [SerializeField]
    private int eventId;

    [SerializeField]
    private float startDelayTime = 0;


    public override void OnEventTrigger(Collider other)
    {
        base.OnEventTrigger(other);
        StartCoroutine(StartText());
    }

    IEnumerator StartText() 
    {

        yield return new WaitForSeconds(startDelayTime);
        while (Managers.Scene.IsTransitioning) 
        {
            yield return null;
        }
        Managers.Game.Player.SignalPanel.Open(eventId);
    }
}
