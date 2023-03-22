using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdle : Singleton<MonsterIdle>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.MosterAnimationIdle();
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
 
public class MonsterPursue : Singleton<MonsterPursue>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.MonsterAnimationWalk();
    }

    public void Exit(MonsterController monsterStateController)
    {
        
    }

    public void FixedUpdateActive(MonsterController monsterStateController)
    {
        monsterStateController.PursuePlayer(Managers.Game.Player.transform);
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
        monsterStateController.MonsterAnimationWalk();
        monsterStateController.Revert();
    }

    public void Exit(MonsterController monsterStateController)
    {
        //monsterStateController.SetMonsterStateIdle();
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

public class MonsterDeath : Singleton<MonsterDeath>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateController)
    {
        monsterStateController.MonsterAnimationDead();
        monsterStateController.Dead();
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
