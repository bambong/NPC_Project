using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserActionEvent : LaserAction
{
    [SerializeField]
    private UnityEvent laserEvent;
    [SerializeField]
    private List<GameObject> laserObject;

    public override void StartLaserEvent()
    {
        if(CheckLaserActive())
        {
            laserEvent?.Invoke();
        }        
    }

    public override void StopLaserEvent()
    {
    }

    private bool CheckLaserActive()
    {
        for(int i = 0; i < laserObject.Count; i++)
        {
            if(laserObject[i].activeSelf == false)
            {
                Debug.Log(laserObject[i].name + i + "is not Active");
                return false;
            }
        }
        return true;
    }
}
