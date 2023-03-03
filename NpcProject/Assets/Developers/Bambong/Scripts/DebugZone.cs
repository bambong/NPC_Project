using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


[RequireComponent(typeof(Collider))]
public class DebugZone : MonoBehaviour
{
    [SerializeField]
    private DebugModCameraController debugModCameraController;

    [SerializeField]
    private GameObject[] keywords;


    private void Awake()
    {
        Managers.Keyword.RegisterDebugZone(this);
        MakeKeyword();
    }

    private void MakeKeyword() 
    { 
        for(int i = 0; i< keywords.Length; ++i) 
        {
            Managers.Keyword.MakeKeywordToDebugZone(this,keywords[i].name);
        }
    
    }

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
            Managers.Game.Player.isDebugButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Keyword.SetDebugZone(null);
            Managers.Game.Player.isDebugButton();
        }
    }
}
