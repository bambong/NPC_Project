using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActionSlow : MonoBehaviour, ILaserAction
{
    public void OnLaserHit()
    {
        Debug.Log("Slow Action");
    }

    public void OffLaserHit()
    {
        Debug.Log("Normal Action");
    }
}
