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
    private float fadeTime = 3.5f;
    private float time = 0;
    private float start = 1f;
    private float end = 0f;

    private Vector3 spawnPoint;

    private SpriteRenderer spriteRenderer;
    private MonsterStateController monsterStateController;
    private NavMeshAgent monsterNav;
    private Animator animator;


    private void Awake()
    {
        monsterStateController = new MonsterStateController(this);
        monsterNav = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        spawnPoint = this.gameObject.transform.position;

        playerDetectController.Init();


        health = 0;
        GetDamaged();
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

    public void Revert()
    {
        monsterNav.SetDestination(spawnPoint);
    }

    public IEnumerator Wait()
    {
        return null;
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
    #endregion





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
