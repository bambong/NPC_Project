using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone"))
        {

        }

    }
}
