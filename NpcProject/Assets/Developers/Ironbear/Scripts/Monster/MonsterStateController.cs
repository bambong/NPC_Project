using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateController : StateController<MonsterContoller>
{
    public MonsterStateController(MonsterContoller monster) : base(monster)
    {
        Init();
    }

    public void Init()
    {
        curState = MonsterIdle.Instance;
    }
}
