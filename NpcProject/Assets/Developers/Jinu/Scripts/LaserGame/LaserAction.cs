using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaserAction : MonoBehaviour, ILaserAction
{
    [SerializeField]
    private float waitTime;

    private int hitcount = 0;
    private float second = 0;

    public void OnLaserHit()
    {
        hitcount++;
        StartCoroutine(PlusTime());
    }

    public void OffLaserHit()
    {
        hitcount--;
        if (hitcount <= 0)
        {
            StartCoroutine(MinusTime());
            StopLaserEvent();
        }
    }

    IEnumerator PlusTime()
    {
        while (waitTime > second && hitcount > 0)
        {
            second += Time.deltaTime;
            yield return null;
            Debug.Log(second);
        }
        if (second >= waitTime)
        {
            second = waitTime;
            StartLaserEvent();
        }
    }

    IEnumerator MinusTime()
    {
        while (second > 0 && hitcount <= 0)
        {
            second -= Time.deltaTime;
            yield return null;
            Debug.Log(second);
        }
        if(second < 0)
        {
            second = 0;
        }
    }

    public abstract void StartLaserEvent();

    public abstract void StopLaserEvent();
}
