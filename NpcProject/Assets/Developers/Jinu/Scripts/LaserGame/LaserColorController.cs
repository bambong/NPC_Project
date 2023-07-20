using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserColorController : MonoBehaviour
{
    [Serializable]
    public struct LaserProperty
    {
        public Material beamMat;
        public Gradient laserColor;
        public Gradient glowboxColor;
    }

    [SerializeField]
    private LaserProperty blueLaser;
    [SerializeField]
    private LaserProperty greenLaer;
    [SerializeField]
    private LaserProperty purpleLaser;
    [SerializeField]
    private LaserProperty redLaser;
    [SerializeField]
    private LaserProperty yellowLaser;

    public LaserProperty GetLaserColor(Define.LaserColor color)
    {
        switch(color)
        {
            case Define.LaserColor.Blue:
                return blueLaser;
            case Define.LaserColor.Green:
                return greenLaer;
            case Define.LaserColor.Purple:
                return purpleLaser;
            case Define.LaserColor.Red:
                return redLaser;
            case Define.LaserColor.Yellow:
                return yellowLaser;
            default:
                return blueLaser;
        }
    }
}
