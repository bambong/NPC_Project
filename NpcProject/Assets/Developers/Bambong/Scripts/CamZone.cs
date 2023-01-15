using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider))]
public class CamZone : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virCamera;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) 
        {
            var camEvent =  new CameraSwitchEvent(virCamera,other.transform);
            camEvent.OnComplete(Managers.Talk.GetTalkEvent(10001));
            camEvent.Play();
        }
    }
}
