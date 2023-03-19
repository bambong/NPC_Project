using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterContoller : MonoBehaviour
{
    [SerializeField]
    private PlayerDetectController playerDetectController;
    [SerializeField]
    private Transform spawnPosition;
    [SerializeField]
    private int health = 100;


    private SpriteRenderer spriteRenderer;
    private MonsterStateController monsterStateController;
    private NavMeshAgent monsterNav;

    [SerializeField]
    private float fadeTime = 2f;
    private float time = 0;
    private float start = 1f;
    private float end = 0f;


    private void Awake()
    {
        monsterStateController = new MonsterStateController(this);
        monsterNav = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerDetectController.Init();
    }

    private void Update()
    {
        monsterStateController.Update();
        if(health<=0)
        {
            SetMonsterStateDeath();
        }
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
        monsterNav.SetDestination(spawnPosition.position);
    }

    public void Dead()
    {
        //animation controll part put here
        StartCoroutine(Invisible());
    }

    IEnumerator Invisible()
    {
        Color fadeColor = spriteRenderer.material.color;
        fadeColor.a = Mathf.Lerp(start, end, time);

        while (fadeColor.a > 0f)
        {
            time += Time.deltaTime / fadeTime;
            fadeColor.a = Mathf.Lerp(start, end, time);
            spriteRenderer.material.color = fadeColor;

            yield return null;
        }
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
