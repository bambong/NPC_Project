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
        Managers.Keyword.SetDebugZone(null);
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
            if(Managers.Game.IsDebugMod) 
            {
                Managers.Game.Player.ExitDebugMod();
                return;
            }
            Managers.Keyword.SetDebugZone(null);
        }
    }
}
