using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectController : MonoBehaviour
{
    public Transform player { get; private set; }

    [SerializeField]
    private string detectionTag = "Player";


    private MonsterController monsterController;

    //private float patienceTime = 0f;
    //private float attackTime = 1f;
    private float distance;
    private float attackRange;
    private bool isPlayer = false;

    public void Init()
    {
        monsterController = GetComponentInParent<MonsterController>();
    }

    private void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, monsterController.spawnPoint);
        
        if (distance <= 0.1f && !isPlayer)
        {
            monsterController.SetMonsterStateIdle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(detectionTag))
        {
            isPlayer = true;
            player = other.gameObject.transform;

            attackRange = Vector3.Distance(player.position, transform.position);

            //monsterController.LeftAndRight(player.position.z);

            if (isPlayer)
            {
                monsterController.SetMonsterStateMove();

                /*
                if (attackRange < 5f)
                {
                    monsterController.SetMonsterStateAttack();
                }
                */
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(detectionTag))
        {
            player = null;
            isPlayer = false;
            if(!isPlayer)
            {
                monsterController.SetMonsterStateRevert();
            }
        }
    }
}
