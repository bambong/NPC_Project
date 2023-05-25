using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System;
using Unity.Mathematics;
using static UnityEngine.Rendering.DebugUI.Table;

public class MonsterController : KeywordEntity , ISpawnAble
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
    private Animator monsterEmoge;
    [Space(1)]
    [Header("Monster Stat")]
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float knockbackForce = 5f;
    [SerializeField]
    private float chaseDistance = 5f;
    [SerializeField]
    private float chaseTime = 5f;
    [Space(1)]
    [Header("Attack Stat")]
    [SerializeField]
    private float attackDistance = 0.5f;
    [Tooltip("X Y = 기본 Box Size 배율 Z 는 실제 Attack Range ")]
    [SerializeField]
    private Vector3 attackBound = new Vector3(1,1,2);
    [SerializeField]
    private float attackSpeed = 1;
    [SerializeField]
    private float attackCoolTime = 0.5f;
    [SerializeField]
    private int attackDamage = 1;
    [Tooltip("공격 거리 몇 퍼센트에서 브레이크를 잡을지 ")]
    [Range(0,1),SerializeField]
    private float attackAutoBreakingAmount = 0.2f;
    [Tooltip("EffectMat")]
    [SerializeField]
    private Material hitMaterial;

    private float waitTime = 2f;
    private int curHealth = 0;
    private float curChaseTime = 0;
    private float curWaitTime = 0;
    private float curAttackTime = 0;
    public bool isPlaySound = true;
    

    private MonsterStateController monsterStateController;
    private Vector3 spawnPos;
    private Transform spawnSpot;
    private SpawnController parentSpawn;
    private int attackLayer = 0;
    private readonly int IDLE_PRIORITY = 99;
    private readonly int REVERT_PRIORITY = 50;
    private readonly int CHASE_PRIORITY = 30;

    private readonly float HIT_EFFECT_TIME = 0.5f;
    private void Awake()
    {
        monsterStateController = new MonsterStateController(this);
        curHealth = maxHealth; 
        chaseDistance += playerDetectController.DetectRange;
        MoveSpeedUpdate();
        monsterNav.enabled = false;
        SetAvoidPriority(IDLE_PRIORITY);
        monsterNav.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        isPlaySound = true;

        playerDetectController.SetActive(false);
        if (spawnSpot == null)
        {
            spawnPos = transform.position;
        }

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
        isPlaySound = true;
        monsterNav.SetDestination(spawnPos);
        FlipToDestination();
    }

    public void GetDamaged()
    {
        curHealth--;
        if (curHealth <= 0)
        {
            Managers.Sound.PlaySFX(Define.SOUND.DeathMonster);
            Managers.Effect.PlayEffect(Define.EFFECT.MonsterDeathEffect , transform);
            DestroyKeywordEntity();
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.HitMonster);
        StartCoroutine(PlayHitEffect());
        KnockBack();
    }
    public void KnockBack()
    {
        //monsterNav.isStopped = true;
        var playerPos = Managers.Game.Player.transform.position;
        Vector3 knockbackDirection = (transform.position - playerPos);
        knockbackDirection.y = 0;
        transform.DOMove(transform.position + knockbackDirection.normalized * knockbackForce,0.1f).SetEase(Ease.OutCirc);
        //monsterRigid.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
    }


    public void FlipToDestination() 
    {
        var det = Vector3.Cross(monsterNav.destination - transform.position , transform.forward);
         spriteRenderer.flipX = 0 > Vector3.Dot( Vector3.up ,det );
    }


    public override void Init()
    {
        SetEmogeChase(false);
        SetEmogeWait(false);
        InitMonsterAttackLayer();
        animator.SetFloat("AttackSpeed", attackSpeed);
        curAttackTime = 0;
        curHealth = maxHealth;
        transform.SetParent(null);
        StartCoroutine(InvokeNextFrame(() => { SetStateIdle(); }));
        playerDetectController.SetActive(true);
        MonsterAnimTimeScaleUpdate();
        base.Init();
    }
    public override void ClearForPool()
    {
        SetStateStop();
        if(parentSpawn != null) 
        {
            parentSpawn.RemoveItem(spawnSpot);
        }
        base.ClearForPool();
    }

    private void SetAvoidPriority(int amount) 
    {
        monsterNav.avoidancePriority = amount;
    }

    public void MoveSpeedUpdate()
    {
        
        monsterNav.speed = moveSpeed * Managers.Time.GetTimeSacle(TIME_TYPE.NONE_PLAYER);
    }
    public override void EnterDebugMod()
    {
        MonsterAnimTimeScaleUpdate();
        MoveSpeedUpdate();
        base.EnterDebugMod();
    }
    public override void ExitDebugMod()
    {
        MonsterAnimTimeScaleClear();
        monsterNav.speed = moveSpeed;
        base.ExitDebugMod();
    }
    public void SetSpawnController(Transform spot, SpawnController controller)
    {
        spawnSpot = spot;
        spawnPos = spawnSpot.transform.position;
        transform.position = spot.position;
        parentSpawn = controller;
    }
    IEnumerator InvokeNextFrame(Action action)
    {
        yield return null;
        action?.Invoke();
    }
    IEnumerator IsMosnterNavEnable(Action action)
    {
        while (!monsterNav.enabled) 
        {
            yield return null;
        }

        action?.Invoke();
    }
    public void AttackTimeFixedUpdate() 
    {
        curAttackTime += Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER);
    }
    public bool IsAttackAble() 
    {
        if (attackCoolTime > curAttackTime)
        {
            return false;
        }
        var dirPlayer = Managers.Game.Player.transform.position - transform.position;
        dirPlayer.y = 0;
        var disToPlayer = dirPlayer.magnitude;
        if (attackDistance >= disToPlayer) 
        {
            var rayDis = monsterNav.destination - transform.position;
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, rayDis.normalized * disToPlayer, Color.blue, 5f);
#endif
            if (!Physics.Raycast(transform.position, rayDis.normalized, disToPlayer, attackLayer,QueryTriggerInteraction.Ignore))
            {
                return true;
            }
        }
        return false;
    }
    private void InitMonsterAttackLayer() 
    {
        attackLayer = 1;
        foreach (var name in Enum.GetNames(typeof(Define.ColiiderMask)))
        {
            if (name == "Player")
            {
                continue;
            }
            attackLayer += (1 << (LayerMask.NameToLayer(name)));
        }
    }

    public void Attack() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.AttackMonster);
        var rayDis = (monsterNav.destination - transform.position);
        rayDis.y = 0;
        rayDis = rayDis.normalized;
        var pos = transform.position + (rayDis * attackBound.z / 2);
        
        int playerLayer = 1 << LayerMask.NameToLayer("Player");
        var boxSize = Util.VectorMultipleScale(new Vector3(col.size.x * attackBound.x, col.size.y * attackBound.y, attackBound.z) /2, transform.lossyScale);
        var rot = Quaternion.LookRotation(rayDis);
#if UNITY_EDITOR
        ExtDebug.DrawBox(pos, boxSize, rot, Color.blue,5f);
#endif
        var hits = Physics.OverlapBox(pos, boxSize, rot, playerLayer, QueryTriggerInteraction.Ignore);
        if (hits.Length < 1)
        {
            return;
        }
        Managers.Game.Player.GetDamage(attackDamage);
    }
    IEnumerator PlayHitEffect() 
    {
        MRenderer.material = hitMaterial;
        yield return new WaitForSeconds(HIT_EFFECT_TIME);
        MRenderer.material = OriginMat;
    }
    public void AttackAnimDone()
    {
        SetStateIdle();
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

    public void MonsterAnimTimeScaleUpdate()
    {
        animator.SetFloat("TimeScale", Managers.Time.GetTimeSacle(TIME_TYPE.NONE_PLAYER));
        animator.SetFloat("AttackSpeed", attackSpeed *Managers.Time.GetTimeSacle(TIME_TYPE.NONE_PLAYER));
    }
    public void MonsterAnimTimeScaleClear()
    {
        animator.SetFloat("TimeScale", 1);
        animator.SetFloat("AttackSpeed", attackSpeed);
    }
    public void SetEmogeWait(bool isOn)
    {
        monsterEmoge.SetBool("IsWait", isOn);
    }
    public void SetEmogeChase(bool isOn)
    {
        monsterEmoge.SetBool("IsChase", isOn);
    }
    public void SetEmogeAttack(bool isOn)
    {
        monsterEmoge.SetBool("IsAttack", isOn);
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
    public void SetStateStop()
    {
        monsterStateController.ChangeState(MonsterStop.Instance);
    }
    #endregion

    #region OnStateEnter
    public void OnStateEnterIdle() 
    {
        SetAvoidPriority(IDLE_PRIORITY);
        MosterAnimationIdle();
    }

    public void OnStateEnterWait()
    {
        SetEmogeWait(true);
        SetAvoidPriority(IDLE_PRIORITY);
        waitTime = UnityEngine.Random.Range(1, 3f);
        monsterNav.isStopped = true;
        MosterAnimationIdle();
    }
    public void OnStateEnterChase()
    {
        SetEmogeChase(true);
        monsterNav.stoppingDistance = attackDistance * attackAutoBreakingAmount;
        SetAvoidPriority(CHASE_PRIORITY);
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
        SetEmogeWait(true);
        SetAvoidPriority(REVERT_PRIORITY);
        MonsterAnimationWalk();
        Revert();
    }
    public void OnStateEnterAttack()
    {
        SetEmogeAttack(true);
        curAttackTime = 0;
        monsterNav.isStopped = true;
        monsterNav.velocity = Vector3.zero;
        playerDetectController.SetActive(false);
        MonsterAnimationAttack(true);
    }
    public void OnStateEnterStop() 
    {
        playerDetectController.SetActive(false);
        monsterNav.isStopped = true;
        monsterNav.enabled = false;
    }

    #endregion

    #region OnStateExit
    public void OnStateExitWait() 
    {
        SetEmogeWait(false);
        curWaitTime = 0;
        monsterNav.isStopped = false;
    }
    public void OnStateExitChase()
    {
        SetEmogeChase(false);
        monsterNav.stoppingDistance = 0;
        curChaseTime = 0;
        playerDetectController.SetActive(true);
    }
    public void OnStateExitStop()
    {
        playerDetectController.SetActive(true);
        monsterNav.enabled = true;
        StartCoroutine(IsMosnterNavEnable(() => { monsterNav.isStopped = false; }));
    }
    public void OnStateExitAttack()
    {
        SetEmogeAttack(false);
        monsterNav.isStopped = false;
        playerDetectController.SetActive(true);
        MonsterAnimationAttack(false);
    }
    public void OnStateExitRevert()
    {
        SetEmogeWait(false);
    }
    #endregion

    #region OnStateFixedUpdate
    public void OnStateFixedUpdateWait()
    {
        AttackTimeFixedUpdate();
        curWaitTime += Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER);
        if (curWaitTime >= waitTime)
        {
            SetStateRevert();
        }
    }
    public void OnStateFixedUpdateRevert()
    {
        AttackTimeFixedUpdate();
        if (0.1f > monsterNav.remainingDistance)
        {
            SetStateIdle();
            return;
        }
    }
    public void OnStateFixedUpdateChase()
    {
        AttackTimeFixedUpdate();
        if (IsAttackAble()) 
        {
            SetStateAttack();
            return;
        }
        
        curChaseTime += Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER);
        monsterNav.avoidancePriority = Mathf.Clamp((int)monsterNav.remainingDistance,1,CHASE_PRIORITY);

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
    public void OnStateFixedUpdateIdle()
    {
        AttackTimeFixedUpdate();
    }
    #endregion
}
