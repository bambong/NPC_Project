using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePratice : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Managers.Sound.ChangeBGM("OMG", 1.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Managers.Sound.ChangeBGM("OMG", 0.0f);
        }
    }
}
