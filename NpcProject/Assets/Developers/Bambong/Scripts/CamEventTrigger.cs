using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CamEventTrigger : EventTrigger
{
    [SerializeField]
    private CinemachineVirtualCamera virCamera;

    public override void OnEventTrigger(Collider other)
    {
        var camEvent = new CameraSwitchEvent(new CameraInfo(virCamera, other.transform));
        camEvent.Play();
        base.OnEventTrigger(other);
    }
}
