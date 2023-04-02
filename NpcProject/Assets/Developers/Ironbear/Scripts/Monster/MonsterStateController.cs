using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateController : StateController<MonsterController>
{
    public MonsterStateController(MonsterController monster) : base(monster)
    {
        Init();
    }
    public void Init()
    {
        curState = MonsterIdle.Instance;
    }
}
