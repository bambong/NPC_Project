using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePratice : MonoBehaviour
{
    float fadeOutDuration = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Managers.Sound.StopBGM();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Managers.Sound.RestartBGM();
        }
    }
}
