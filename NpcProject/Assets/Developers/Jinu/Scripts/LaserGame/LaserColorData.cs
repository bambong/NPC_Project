using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserColorData", menuName = "Scriptable Data/LaserColorData", order = 4)]
public class LaserColorData : ScriptableObject
{
    public List<LaserColor> laserColors;

    public LaserColor GetLaserColor(Define.LaserColor color)
    {
        switch (color)
        {
            case Define.LaserColor.Blue:
                return laserColors[0];
            case Define.LaserColor.Green:
                return laserColors[1];
            case Define.LaserColor.Purple:
                return laserColors[2];
            case Define.LaserColor.Red:
                return laserColors[3];
            case Define.LaserColor.Yellow:
                return laserColors[4];
            default:
                return laserColors[0];
        }
    }
}