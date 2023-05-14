using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : StateController<PlayerController>
{
    public PlayerStateController(PlayerController player) : base(player)
    {
        Init();
    }
    public void Init()
    {
        curState = PlayerIdle.Instance;
    }
}
