using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationEventTrigger : EventTrigger
{
    [SerializeField]
    private string destinationText;

    [SerializeField]
    private float startDelayTime = 0.5f;


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
        Managers.Game.OpenDestination(destinationText);
    }
}
