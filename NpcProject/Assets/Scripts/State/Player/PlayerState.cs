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
    }
}

public class PlayerMove : Singleton<PlayerMove>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.AnimRunEnter();
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
    }

    public void UpdateActive(PlayerController stateController)
    {
    }
}

public class PlayerInteraction : Singleton<PlayerInteraction>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.InteractionEnter();
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
public class PlayerKeywordMod : Singleton<PlayerKeywordMod>, IState<PlayerController>
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
    }

    public void UpdateActive(PlayerController stateController)
    {
        stateController.KeywordModInputCheck();
    }
}