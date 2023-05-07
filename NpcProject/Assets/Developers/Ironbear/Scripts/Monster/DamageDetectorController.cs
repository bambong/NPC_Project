using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetectorController : MonoBehaviour
{
    public Transform player { get; private set; }

    [SerializeField]
    private string detectionTag = "Player";


    private MonsterController monsterController;

    public void Init()
    {
        monsterController = GetComponentInParent<MonsterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(detectionTag))
        {
            monsterController.SetStateDamaged();
        }
    }
}
