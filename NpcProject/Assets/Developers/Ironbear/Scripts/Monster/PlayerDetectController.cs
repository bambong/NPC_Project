using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectController : MonoBehaviour
{
    public Transform player { get; private set; }

    [SerializeField]
    private string detectionTag = "Player";


    private MonsterController monsterController;

    private float attackTime = 2f;
    private float patienceTime = 0f;
    private float distance;
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
            patienceTime += Time.deltaTime;
            player = other.gameObject.transform;
            isPlayer = true;
            if(isPlayer)
            {
                monsterController.SetMonsterStateMove();
            }
            else if(isPlayer && patienceTime==attackTime)
            {
                Debug.Log(patienceTime);
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
