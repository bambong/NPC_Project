using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterContoller : MonoBehaviour
{
    [SerializeField]
    private PlayerDetectController playerDetectController;

    private MonsterStateController monsterStateController;
    private NavMeshAgent monsterNav;


    private void Awake()
    {
        monsterStateController = new MonsterStateController(this);
        monsterNav = GetComponent<NavMeshAgent>();
        playerDetectController.Init();
    }

    private void Update()
    {
        monsterStateController.Update();
    }

    private void FixedUpdate()
    {
        monsterStateController.FixedUpdate();
    }

    public void PursuePlayer(Transform player)
    {
        monsterNav.SetDestination(player.position);
    }

    public void Revert(Transform monster)
    {
        monsterNav.SetDestination(monster.position);
    }

    #region SetMonsterState
    public void SetMonsterStateIdle()
    {
        monsterStateController.ChangeState(MonsterIdle.Instance);
    }

    public void SetMonsterStatePursue()
    {
        monsterStateController.ChangeState(MonsterPursue.Instance);
    }

    public void SetMonsterStateRevert()
    {
        monsterStateController.ChangeState(MonsterRevert.Instance);
    }

    public void SetMonsterStateAttack()
    {
        monsterStateController.ChangeState(MonsterAttack.Instance);
    }

    public void SetMonsterStateDeath()
    {
        monsterStateController.ChangeState(MonsterDeath.Instance);
    }
    #endregion
}
