using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BgmZone : MonoBehaviour
{
    [SerializeField]
    private string areaName;

    private SoundController soundController;

    public void OnEnterBgmZone()
    {
        
    }

    public void OnExitBgmZone()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Managers.Sound.CheckBgmZone(this);
            //여기서?!!?!?
            //bgm 볼륨 서서히 키우기(?)
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Managers.Sound.CheckBgmZone(null);
            //bgm 볼륨 서서히 줄이기(?)
        }
    }
}
