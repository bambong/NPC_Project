using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


[RequireComponent(typeof(Collider))]
public class DebugZone : MonoBehaviour
{
    [SerializeField]
    private DebugModCameraController debugModCameraController;

    public void OnEnterDebugMod() 
    {
        debugModCameraController.EnterDebugMod();
    }
    public void OnExitDebugMod() 
    {
        debugModCameraController.ExitDebugMod();
        Managers.Camera.SwitchPrevCamera();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Keyword.SetDebugZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Keyword.SetDebugZone(null);
        }
    }
}
