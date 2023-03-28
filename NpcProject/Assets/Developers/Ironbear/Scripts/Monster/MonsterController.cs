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
    private float knockbackForce = 1f;

    [SerializeField]
    private float fadeTime = 3.5f;
    private float effectDuration = 0;
    private float start = 1f;
    private float end = 0f;

    private float waitTime = 0f;

    public string detectionTag = "Player";
    private float attackTime = 2f;
    private float patienceTime = 0f;
    private bool isPlayer = false;

    private SpriteRenderer spriteRenderer;
    private MonsterStateController monsterStateController;
    private NavMeshAgent monsterNav;
    private Animator animator;
    private Rigidbody monsterRigid;
    private GameObject player;
    

    [HideInInspector]
    public Vector3 spawnPoint;

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

    IEnumerator Invisible()
    {
        Color fadeColor = spriteRenderer.material.color;
        fadeColor.a = Mathf.Lerp(start, end, effectDuration);

        yield return new WaitForSeconds(0.35f);

        while (fadeColor.a > 0f)
        {
            effectDuration += Time.deltaTime / fadeTime;
            fadeColor.a = Mathf.Lerp(start, end, effectDuration);
            spriteRenderer.material.color = fadeColor;

            yield return null;
        }
    }


    #region SetMonsterAnimation
    public void MosterAnimationIdle()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isAttack", false);
    }

    public void MonsterAnimationWalk()
    {
        animator.SetBool("isWalk", true);
        animator.SetBool("isAttack", false);
    }

    public void MonsterAnimationAttack()
    {
        animator.SetBool("isAttack", true);
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
