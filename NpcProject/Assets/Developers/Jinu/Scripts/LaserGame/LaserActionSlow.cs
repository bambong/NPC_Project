using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActionSlow : MonoBehaviour, ILaserAction
{
    [SerializeField]
    private TIME_TYPE slowType;
    [SerializeField]
    private float slowTime;

    private bool onHit;

    public void OnLaserHit()
    {
        Managers.Time.SetTimeScale(slowType, 0.5f);
        //Managers.Game.Player.
        onHit = true;
        Debug.Log("Slow Action");
        StartCoroutine(Slow());
    }

    public void OffLaserHit()
    {
        onHit = false;
    }

    IEnumerator Slow()
    {
        while(onHit)
        {
            yield return null;
        }
        yield return new WaitForSeconds(slowTime);
        Managers.Time.SetTimeScale(slowType, 1.0f);
        Debug.Log("Normal Action");
    }
}
