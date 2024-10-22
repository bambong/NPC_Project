using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaserAction : MonoBehaviour, ILaserAction
{
    [SerializeField]
    private GameObject parent;
    [SerializeField]
    protected float waitTime = 2.0f;

    protected float second = 0;
    [SerializeField]
    protected int hitcount = 0;
    

    public void OnLaserHit()
    {
        hitcount++;
        if(hitcount == 1)
        {
            StartCoroutine(PlusTime());
        }
    }

    public virtual void OffLaserHit()
    {
        hitcount--;
        if (hitcount <= 0)
        {
            hitcount = 0;
            if (!parent.activeSelf)
            {
                return;
            }
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

    protected IEnumerator MinusTime()
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
