using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIdle : Singleton<PlayerIdle>, IState<PlayerController>
{

    public void Enter(PlayerController stateController)
    {
        stateController.SetAnimIdle();
    }

    public void Exit(PlayerController stateController)
    {
    }

    public void FixedUpdateActive(PlayerController stateController)
    {
    }

    public void UpdateActive(PlayerController stateController)
    {
        stateController.PlayerInputCheck();
    }
}

public class PlayerMove : Singleton<PlayerMove>, IState<PlayerController>
{

    public void Enter(PlayerController stateController)
    {
        stateController.SetAnimRun();
    }

    public void Exit(PlayerController stateController)
    {
    }

    public void FixedUpdateActive(PlayerController stateController)
    {

    }

    public void UpdateActive(PlayerController stateController)
    {
        stateController.PlayerMoveUpdate();
    }
}

public class PlayerStop : Singleton<PlayerStop>, IState<PlayerController>
{

    public void Enter(PlayerController stateController)
    {
    }

    public void Exit(PlayerController stateController)
    {
    }

    public void FixedUpdateActive(PlayerController stateController)
    {
    }

    public void UpdateActive(PlayerController stateController)
    {
    }
}

public class PlayerInteraction : Singleton<PlayerInteraction>, IState<PlayerController>
{
    public void Enter(PlayerController stateController)
    {
    }

    public void Exit(PlayerController stateController)
    {
    }

    public void FixedUpdateActive(PlayerController stateController)
    {
    }

    public void UpdateActive(PlayerController stateController)
    {
    }
}