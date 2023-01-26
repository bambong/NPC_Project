using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class TimeManager
{
    public float CoroutineTimeScale()
    {
        return Time.timeScale;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public float GetTimeScale(bool isChange)
    {
        if(isChange == true)
        {
            return Time.timeScale;
        }
        else
        {
            return Time.unscaledDeltaTime;
        }
    }

}
