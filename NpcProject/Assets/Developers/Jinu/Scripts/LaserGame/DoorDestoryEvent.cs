using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorDestoryEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject talkTrigger;
    public void Update()
    {        
        if(door.activeInHierarchy)
        {
            return;
        }
        else
        {
            talkTrigger.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
