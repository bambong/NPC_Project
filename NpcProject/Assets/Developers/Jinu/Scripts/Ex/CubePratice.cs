using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePratice : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Managers.Sound.BGMControl(Define.BGM.Pause);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Managers.Sound.BGMControl(Define.BGM.ReStart);
        }
    }
}
