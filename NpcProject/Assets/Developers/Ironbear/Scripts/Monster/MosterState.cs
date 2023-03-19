using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdle : Singleton<MonsterIdle>, IState<MonsterContoller>
{
    public void Init()
    {

    }

    public void Enter(MonsterContoller monsterStateContoller)
    {

    }

    public void Exit(MonsterContoller monsterStateContoller)
    {

    }

    public void FixedUpdateActive(MonsterContoller monsterStateContoller)
    {

    }

    public void UpdateActive(MonsterContoller mmonsterStateController)
    {

    }
}

public class MonsterPursue : Singleton<MonsterPursue>, IState<MonsterContoller>
{
    public void Init()
    {

    }

    public void Enter(MonsterContoller monsterStateContoller)
    {

    }

    public void Exit(MonsterContoller monsterStateContoller)
    {
        monsterStateContoller.Revert();
    }

    public void FixedUpdateActive(MonsterContoller monsterStateContoller)
    {
        monsterStateContoller.PursuePlayer(Managers.Game.Player.transform);
    }

    public void UpdateActive(MonsterContoller mmonsterStateController)
    {

    }
}

public class MonsterRevert : Singleton<MonsterRevert>, IState<MonsterContoller>
{
    public void Init()
    {

    }

    public void Enter(MonsterContoller monsterStateContoller)
    {

    }

    public void Exit(MonsterContoller monsterStateContoller)
    {

    }

    public void FixedUpdateActive(MonsterContoller monsterStateContoller)
    {

    }

    public void UpdateActive(MonsterContoller mmonsterStateController)
    {

    }
}

public class MonsterAttack : Singleton<MonsterAttack>, IState<MonsterContoller>
{
    public void Init()
    {

    }

    public void Enter(MonsterContoller monsterStateContoller)
    {

    }

    public void Exit(MonsterContoller monsterStateContoller)
    {

    }

    public void FixedUpdateActive(MonsterContoller monsterStateContoller)
    {

    }

    public void UpdateActive(MonsterContoller mmonsterStateController)
    {

    }
}

public class MonsterDeath : Singleton<MonsterDeath>, IState<MonsterContoller>
{
    public void Init()
    {

    }

    public void Enter(MonsterContoller monsterStateContoller)
    {
        monsterStateContoller.Dead();
    }

    public void Exit(MonsterContoller monsterStateContoller)
    {

    }

    public void FixedUpdateActive(MonsterContoller monsterStateContoller)
    {

    }

    public void UpdateActive(MonsterContoller mmonsterStateController)
    {

    }
}
