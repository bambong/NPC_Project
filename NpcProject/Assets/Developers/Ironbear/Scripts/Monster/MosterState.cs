using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdle : Singleton<MonsterIdle>, IState<MonsterController>
{
    public void Init()
    {

    }

    public void Enter(MonsterController monsterStateContoller)
    {

    }

    public void Exit(MonsterController monsterStateContoller)
    {

    }

    public void FixedUpdateActive(MonsterController monsterStateContoller)
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
        monsterStateController.MosterAnimationIdle();
        monsterStateController.Revert();
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

    public void Enter(MonsterController monsterStateContoller)
    {

    }

    public void Exit(MonsterController monsterStateContoller)
    {

    }

    public void FixedUpdateActive(MonsterController monsterStateContoller)
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

    public void Enter(MonsterController monsterStateContoller)
    {

    }

    public void Exit(MonsterController monsterStateContoller)
    {

    }

    public void FixedUpdateActive(MonsterController monsterStateContoller)
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

    public void Enter(MonsterController monsterStateContoller)
    {
        monsterStateContoller.MonsterAnimationDead();
        monsterStateContoller.Dead();
    }

    public void Exit(MonsterController monsterStateContoller)
    {

    }

    public void FixedUpdateActive(MonsterController monsterStateContoller)
    {

    }

    public void UpdateActive(MonsterController mmonsterStateController)
    {

    }
}
