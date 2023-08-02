using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaserAction : MonoBehaviour, ILaserAction
{
    [SerializeField]
    private float waitTime;
    [SerializeField]
    private int hitcount = 0;
    [SerializeField]
    private float second = 0;
    [SerializeField]
    private bool isPlay = false;

    public void OnLaserHit()
    {
        hitcount++;
        if(!isPlay)
        {
            isPlay = true;
            StartCoroutine(PlusTime());
        }
    }

    public void OffLaserHit()
    {
        hitcount--;
        if (hitcount <= 0)
        {
            isPlay = false;
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
