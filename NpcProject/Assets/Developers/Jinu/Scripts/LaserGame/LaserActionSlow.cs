using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActionSlow : MonoBehaviour, ILaserAction
{
    private const float NORMAL_SPEED = 1.0f;
    [SerializeField]
    private TIME_TYPE slowType;
    [SerializeField]
    private float slowSpeed = 0.5f;
    [SerializeField]
    private float slowTime;

    public static int actionStack = 0;

    private bool onHit;

    public void OnLaserHit()
    {
        Managers.Time.SetTimeScale(slowType, slowSpeed);
        onHit = true;
        actionStack++;
        StartCoroutine(Slow());
        Debug.Log("Start Slow Action");
    }

    public void OffLaserHit()
    {
        onHit = false;
    }

    IEnumerator Slow()
    {
        while (onHit)
        {
            yield return null;
        }
        yield return new WaitForSeconds(slowTime);

        actionStack--;

        if (actionStack == 0)
        {
            Managers.Time.SetTimeScale(slowType, NORMAL_SPEED);
            Debug.Log("End Slow Action");
        }
        else
        {
            Debug.Log("Keep Slow Action");
        }
    }
}