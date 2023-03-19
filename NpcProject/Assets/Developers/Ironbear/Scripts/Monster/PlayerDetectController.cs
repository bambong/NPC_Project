using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectController : MonoBehaviour
{

    public bool playerInArea { get; private set; }
    public Transform player { get; private set; }

    [SerializeField]
    private string detectionTag = "Player";


    private MonsterContoller monsterContoller;
    private MonsterStateController monsterStateController;

    private bool isPlayer = false;

    public void Init()
    {
        monsterContoller = GetComponentInParent<MonsterContoller>();
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
                monsterContoller.SetMonsterStatePursue();
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
                monsterContoller.SetMonsterStateRevert();
            }
        }
    }
}
