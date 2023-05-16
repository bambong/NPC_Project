using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public enum TIME_TYPE
{
    PLAYER,
    NONE_PLAYER
}

public class TimeManager
{    
    Dictionary<TIME_TYPE, float> timeScale = new Dictionary<TIME_TYPE, float>()
    {
        {TIME_TYPE.PLAYER, 1.0f},
        {TIME_TYPE.NONE_PLAYER, 1.0f}
    };

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void SetTimeScale(TIME_TYPE type, float scale)
    {
        timeScale[type] = scale;
    }

    public float GetTime(bool isDelta)
    {
        if (isDelta == true)
        {
            return Time.deltaTime;
        }
        else
        {
            return Time.unscaledDeltaTime;
        }
    }

    public float GetDeltaTime(float scale)
    {
        return Time.deltaTime * scale;
    }

    public float GetDeltaTime(TIME_TYPE type)
    {
        return Time.deltaTime * timeScale[type];
    }

    public float GetFixedDeltaTime(TIME_TYPE type)
    {
        return Time.fixedDeltaTime * timeScale[type];
    }
    public float GetTimeSacle(TIME_TYPE type)
    {
        return timeScale[type];
    }
    public void TimeScaleClear(float scale)
    {
        foreach (var temp in timeScale) 
        {
            timeScale[temp.Key] = scale;
        }
    }
}
