using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MonsterController : KeywordEntity
{
    [Header("Element")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private NavMeshAgent monsterNav;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody monsterRigid;
    [SerializeField]
    private PlayerDetectController playerDetectController;
    [SerializeField]
    private BoxCollider attackRange;
    [Space(1)]
    [Header("Monster Stat")]
    [SerializeField]
    private int health = 100;
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float knockbackForce = 5f;
    [SerializeField]
    private float chaseDistance = 5f;
    [SerializeField]
    private float chaseTime = 5f;

    [HideInInspector]
    public Vector3 spawnPoint;

    private float waitTime = 2f;
    private int curHealth = 0;
    private float curChaseTime = 0;
    private float curWaitTime = 0;
    private MonsterStateController monsterStateController;

    private void Awake()
    {
        monsterStateController = new MonsterStateController(this);
        curHealth = health;
        // monsterRigid = GetComponent<Rigidbody>();
        MoveSpeedUpdate();
        spawnPoint = transform.position;
        chaseDistance += playerDetectController.DetectRange;
        playerDetectController.Init();
    }

    private void Update()
    {
        monsterStateController.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        monsterStateController.FixedUpdate();
    }
    public void LateUpdate()
    {
        var rotY = Camera.main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0,rotY,0);
    }

    public void ChasePlayer(Transform player)
    {
        monsterNav.SetDestination(player.position);
        FlipToDestination();
    }

    public void Revert()
    {
        monsterNav.SetDestination(spawnPoint);
        FlipToDestination();
    }

    public void GetDamaged()
    {
        curHealth--;
        if (curHealth <= 0)
        {
            SetStateDeath();
        }
    }
    public void KnockBack()
    {
        var playerPos = Managers.Game.Player.transform.position;
        Vector3 knockbackDirection = (transform.position - playerPos).normalized;
        monsterRigid.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }
    public void AttackRangeActive()
    {
        attackRange.gameObject.SetActive(true);
    }
    public void AttackRangeDeactive()
    {
        attackRange.gameObject.SetActive(false);
    }

    public void FlipToDestination() 
    {
        var det = Vector3.Cross(monsterNav.destination - transform.position , transform.forward);
         spriteRenderer.flipX = 0 > Vector3.Dot( Vector3.up ,det );
    }
    public override void DestroyKeywordEntity()
    {
        monsterNav.isStopped = true;
        SetStateDeath();
        base.DestroyKeywordEntity();
    }
    public void MoveSpeedUpdate()
    {
        monsterNav.speed = moveSpeed * Managers.Time.GetTimeSacle(TIME_TYPE.NONE_PLAYER);
    }
    public override void EnterDebugMod()
    {
        MoveSpeedUpdate();
        base.EnterDebugMod();
    }
    public override void ExitDebugMod()
    {
        monsterNav.speed = moveSpeed;
        base.ExitDebugMod();
    }

    #region SetMonsterAnimation
    public void MosterAnimationIdle()
    {
        animator.SetBool("isWalk", false);
    }

    public void MonsterAnimationWalk()
    {
        animator.SetBool("isWalk", true);
    }

    public void MonsterAnimationDead()
    {
        animator.SetTrigger("isDead");
    }

    public void MonsterAnimationAttack(bool isON)
    {
        animator.SetBool("isAttack", isON);
    }
    #endregion

    #region SetState
    public void SetStateIdle()
    {
        monsterStateController.ChangeState(MonsterIdle.Instance);
    }

    public void SetStateChase()
    {
        monsterStateController.ChangeState(MonsterChase.Instance);
    }

    public void SetStateRevert()
    {
        monsterStateController.ChangeState(MonsterRevert.Instance);
    }

    public void SetStateAttack()
    {
        monsterStateController.ChangeState(MonsterAttack.Instance);
    }

    public void SetStateDamaged()
    {
        monsterStateController.ChangeState(MonsterDamaged.Instance);
    }

    public void SetStateWait()
    {
        monsterStateController.ChangeState(MonsterWait.Instance);
    }
    public void SetStateDeath()
    {
        monsterStateController.ChangeState(MonsterDeath.Instance);
    }
    #endregion

    #region OnStateEnter
    public void OnStateEnterIdle() 
    {
        MosterAnimationIdle();
    }

    public void OnStateEnterWait()
    {
        waitTime = Random.Range(1, 3f);
        monsterNav.isStopped = true;
        MosterAnimationIdle();
    }
    public void OnStateEnterChase()
    {
        playerDetectController.SetActive(false);
        MonsterAnimationWalk();
    }
    public void OnStateEnterDead()
    {
        MonsterAnimationDead();
    }
    public void OnStateEnterDamaged()
    {
        GetDamaged();
        KnockBack();
    }
    public void OnStateEnterRevert()
    {
        MonsterAnimationWalk();
        Revert();
    }

    #endregion

    #region OnStateExit
    public void OnStateExitWait() 
    {
        curWaitTime = 0;
        monsterNav.isStopped = false;
    }
    public void OnStateExitChase()
    {
        curChaseTime = 0;
        playerDetectController.SetActive(true);
    }
    #endregion

    #region OnStateFixedUpdate
    public void OnStateFixedUpdateWait()
    {
        curWaitTime += Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER);
        if (curWaitTime >= waitTime)
        {
            SetStateRevert();
        }
    }
    public void OnStateFixedUpdateRevert()
    {
        if (0.1f > monsterNav.remainingDistance)
        {
            SetStateIdle();
            return;
        }
    }
    public void OnStateFixedUpdateChase()
    {

        curChaseTime += Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER);

        if (curChaseTime >= chaseTime)
        {
            if(chaseDistance < monsterNav.remainingDistance) 
            {
                SetStateWait();
                return;
            }
        }
        ChasePlayer(Managers.Game.Player.transform);
    }
    #endregion
}
