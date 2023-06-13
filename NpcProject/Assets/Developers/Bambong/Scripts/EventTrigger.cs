using FMOD;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EventTrigger : GuIdBehaviour
{
    [SerializeField]
    private bool guidEventPlayOnce = false;

    [SerializeField]
    private string targetTag = "Player";

    [SerializeField]
    private bool isPlayOnce = true;

    [SerializeField]
    private UnityEvent onInteract;

    protected bool isPlay = false;



    public virtual void OnEventTrigger(Collider other) 
    {
        isPlay = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag)) 
        {
            if (guidEventPlayOnce) 
            {
                if (Managers.Data.IsClearEvent(guId)) 
                {
                    return;
                }
                Managers.Data.ClearEvent(guId);

            }

            if (isPlay && isPlayOnce)
            {
                return;
            }
            onInteract?.Invoke();
            OnEventTrigger(other);
        }
    }
}
