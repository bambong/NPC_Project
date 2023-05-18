using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleUI : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.Game.Player.GetDamage(4);
        }
    }
}
