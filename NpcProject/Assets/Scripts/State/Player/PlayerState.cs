using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIdle : Singleton<PlayerIdle>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.OpenPlayerUI();
        stateController.AnimIdleEnter();
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
        stateController.DebugModEnterInputCheck();
    }
}

public class PlayerMove : Singleton<PlayerMove>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
    }

    public void Exit(PlayerController stateController)
    {
    }

    public void FixedUpdateActive(PlayerController stateController)
    {
        stateController.PlayerMoveUpdate();
    }

    public void UpdateActive(PlayerController stateController)
    {
        stateController.InteractionInputCheck();
        stateController.DebugModEnterInputCheck();
    }
}

public class PlayerStop : Singleton<PlayerStop>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.ClosePlayerUI();
        stateController.ClearMoveAnim();        
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
public class PlayerDebugMod : Singleton<PlayerDebugMod>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.ClosePlayerUI();
    }

    public void Exit(PlayerController stateController)
    {
    }

    public void FixedUpdateActive(PlayerController stateController)
    {
    }

    public void UpdateActive(PlayerController stateController)
    {
        stateController.DebugModEnterInputCheck();
    }
}

public class PlayerInteraction : Singleton<PlayerInteraction>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.ClearMoveAnim();
        stateController.InteractionEnter();
        stateController.ClosePlayerUI();
    }

    public void Exit(PlayerController stateController)
    {
        stateController.InteractionExit();
    }

    public void FixedUpdateActive(PlayerController stateController)
    {
    }

    public void UpdateActive(PlayerController stateController)
    {
    }
}

public class PlayerDeath : Singleton<PlayerDeath>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.OpenDeathUI();
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