using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            Managers.Sound.areaCheck(other.gameObject.name);
        }

    }
}
