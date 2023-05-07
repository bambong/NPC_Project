using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterStop : Singleton<MonsterStop>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.OnStateEnterStop();
    }

    public void Exit(MonsterController monsterStateController)
    {
        monsterStateController.OnStateExitStop();
    }

    public void FixedUpdateActive(MonsterController monsterStateController)
    {

    }

    public void UpdateActive(MonsterController mmonsterStateController)
    {

    }
}

public class MonsterIdle : Singleton<MonsterIdle>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.OnStateEnterIdle();
    }

    public void Exit(MonsterController monsterStateController)
    {

    }

    public void FixedUpdateActive(MonsterController monsterStateController)
    {
        monsterStateController.OnStateFixedUpdateIdle();
    }

    public void UpdateActive(MonsterController mmonsterStateController)
    {

    }
}
 
public class MonsterChase : Singleton<MonsterChase>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.OnStateEnterChase();
    }

    public void Exit(MonsterController monsterStateController)
    {
        monsterStateController.OnStateExitChase();
    }

    public void FixedUpdateActive(MonsterController monsterStateController)
    {
        monsterStateController.OnStateFixedUpdateChase();
    }

    public void UpdateActive(MonsterController monsterStateController)
    {
        
    }
}

public class MonsterRevert : Singleton<MonsterRevert>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.OnStateEnterRevert();
    }

    public void Exit(MonsterController monsterStateController)
    {
        monsterStateController.OnStateExitRevert();
    }

    public void FixedUpdateActive(MonsterController monsterStateController)
    {
        monsterStateController.OnStateFixedUpdateRevert();
    }

    public void UpdateActive(MonsterController mmonsterStateController)
    {

    }
}

public class MonsterDamaged : Singleton<MonsterDamaged>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.OnStateEnterDamaged();
    }

    public void Exit(MonsterController monsterStateController)
    {

    }

    public void FixedUpdateActive(MonsterController monsterStateController)
    {

    }

    public void UpdateActive(MonsterController mmonsterStateController)
    {

    }
}

public class MonsterAttack : Singleton<MonsterAttack>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.OnStateEnterAttack();
    }

    public void Exit(MonsterController monsterStateController)
    {
        monsterStateController.OnStateExitAttack();
    }

    public void FixedUpdateActive(MonsterController monsterStateController)
    {

    }

    public void UpdateActive(MonsterController mmonsterStateController)
    {

    }
}
public class MonsterWait : Singleton<MonsterWait>, IState<MonsterController>
{
    public void Init()
    {
    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.OnStateEnterWait();
    }

    public void Exit(MonsterController monsterStateController)
    {
        monsterStateController.OnStateExitWait();
    }

    public void FixedUpdateActive(MonsterController monsterStateController)
    {
        monsterStateController.OnStateFixedUpdateWait();
    }

    public void UpdateActive(MonsterController mmonsterStateController)
    {
    }
}
public class MonsterDeath : Singleton<MonsterDeath>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.OnStateEnterDead();
    }

    public void Exit(MonsterController monsterStateController)
    {

    }

    public void FixedUpdateActive(MonsterController monsterStateController)
    {

    }

    public void UpdateActive(MonsterController mmonsterStateController)
    {

    }
}
