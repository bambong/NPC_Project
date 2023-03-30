using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private PlayerDetectController playerDetectController;
    [SerializeField]
    private int health = 100;

    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float knockbackForce = 5f;

    [SerializeField]
    private float fadeTime = 3.5f;
    private float time = 0;
    private float start = 1f;
    private float end = 0f;

    private float waitTime = 0f;

    [HideInInspector]
    public Vector3 spawnPoint;
    [SerializeField]
    private BoxCollider attackRange;


    private SpriteRenderer spriteRenderer;
    private MonsterStateController monsterStateController;
    private NavMeshAgent monsterNav;
    private Animator animator;
    private Rigidbody monsterRigid;
    private GameObject player;


    private void Awake()
    {
        monsterStateController = new MonsterStateController(this);
        monsterNav = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        monsterRigid = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        monsterNav.speed = moveSpeed;
        spawnPoint = this.gameObject.transform.position;

        playerDetectController.Init();
    }

    private void Update()
    {
        monsterStateController.Update();
    }

    private void FixedUpdate()
    {
        monsterStateController.FixedUpdate();
        waitTime = Random.Range(0f, 2f);
    }





    public void PursuePlayer(Transform player)
    {
        monsterNav.SetDestination(player.position);
    }

    public void Revert()
    {
        monsterNav.speed = 1f;
        monsterNav.SetDestination(spawnPoint);
    }

    public IEnumerator Wait()
    {
        monsterNav.speed = 0f;
        
        yield return new WaitForSeconds(waitTime);

        MonsterAnimationWalk();
        Revert();
        LeftAndRightSpawnPoint();
    }

    public void Dead()
    {
        StartCoroutine(Invisible());
    }

    public void GetDamaged()
    {
        if (health == 0)
        {
            SetMonsterStateDeath();
        }
    }

    public void KnockBack()
    {
        Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
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

    public void LeftAndRightPlayer(Transform player)
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        if (lookDirection.z < 0f)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void LeftAndRightSpawnPoint()
    {
        Vector3 lookDirection = (spawnPoint - transform.position).normalized;

        if (lookDirection.z < 0f)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    IEnumerator Invisible()
    {
        Color fadeColor = spriteRenderer.material.color;
        fadeColor.a = Mathf.Lerp(start, end, time);

        yield return new WaitForSeconds(0.35f);

        while (fadeColor.a > 0f)
        {
            time += Time.deltaTime / fadeTime;
            fadeColor.a = Mathf.Lerp(start, end, time);
            spriteRenderer.material.color = fadeColor;

            yield return null;
        }
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
    #endregion


    #region SetMonsterState
    public void SetMonsterStateIdle()
    {
        monsterStateController.ChangeState(MonsterIdle.Instance);
    }

    public void SetMonsterStateMove()
    {
        monsterStateController.ChangeState(MonsterMove.Instance);
    }

    public void SetMonsterStateRevert()
    {
        monsterStateController.ChangeState(MonsterRevert.Instance);
    }

    public void SetMonsterStateAttack()
    {
        monsterStateController.ChangeState(MonsterAttack.Instance);
    }

    public void SetMonsterStateDamaged()
    {
        monsterStateController.ChangeState(MonsterDamaged.Instance);
    }

    public void SetMonsterStateDeath()
    {
        monsterStateController.ChangeState(MonsterDeath.Instance);
    }
    #endregion
}
