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
            //���⼭?!!?!?
            //bgm ���� ������ Ű���(?)
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Managers.Sound.CheckBgmZone(null);
            //bgm ���� ������ ���̱�(?)
        }
    }
}
