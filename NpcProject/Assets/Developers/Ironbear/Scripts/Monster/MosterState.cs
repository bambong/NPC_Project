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

public class MonsterPursue : Singleton<MonsterIdle>, IState<MonsterContoller>
{
    public void Init()
    {

    }

    public void Enter(MonsterContoller monsterStateContoller)
    {
        monsterStateContoller.PursuePlayer(Managers.Game.Player.transform);
    }

    public void Exit(MonsterContoller monsterStateContoller)
    {
        monsterStateContoller.Revert(Managers.Game.Player.transform);
    }

    public void FixedUpdateActive(MonsterContoller monsterStateContoller)
    {

    }

    public void UpdateActive(MonsterContoller mmonsterStateController)
    {

    }
}

public class MonsterRevert : Singleton<MonsterIdle>, IState<MonsterContoller>
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

public class MonsterAttack : Singleton<MonsterIdle>, IState<MonsterContoller>
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

public class MonsterDeath : Singleton<MonsterIdle>, IState<MonsterContoller>
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
