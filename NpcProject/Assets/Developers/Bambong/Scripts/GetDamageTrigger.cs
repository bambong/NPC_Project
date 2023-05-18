using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamageTrigger : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 1;
    private bool isPlay = false;
    private void OnEnable()
    {
        isPlay = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlay) 
        {
            isPlay = true;
            Managers.Game.Player.GetDamage(damageAmount);
        }
    }
}
