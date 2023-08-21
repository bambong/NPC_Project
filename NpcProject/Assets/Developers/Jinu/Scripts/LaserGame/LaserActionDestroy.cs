using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActionDestroy : LaserAction
{
    [SerializeField]
    private GameObject destroyObj;

    public override void StartLaserEvent()
    {
        destroyObj.SetActive(false);
        Destroy(this);
    }

    public override void StopLaserEvent()
    {
        
    }
}
