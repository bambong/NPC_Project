using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectController : MonoBehaviour
{

    public bool playerInArea { get; private set; }
    public Transform player { get; private set; }

    [SerializeField]
    private string detectionTag = "Player";


    private MonsterController monsterController;

    private bool isPlayer = false;

    public void Init()
    {
        monsterController = GetComponentInParent<MonsterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(detectionTag))
        {
            playerInArea = true;
            player = other.gameObject.transform;
            isPlayer = true;
            if(isPlayer)
            {
                monsterController.SetMonsterStatePursue();
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(detectionTag))
        {
            playerInArea = false;
            player = null;
            isPlayer = false;
            if(!isPlayer)
            {
                monsterController.SetMonsterStateRevert();
            }
        }
    }
}
