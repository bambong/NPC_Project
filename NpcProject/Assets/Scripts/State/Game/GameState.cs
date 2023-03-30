using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNormalState : Singleton<GameNormalState>,IState<GameManager>
{
    public void Init()
    {
    }

    public void Enter(GameManager stateController)
    {
    }

    public void Exit(GameManager stateController)
    {
    }

    public void FixedUpdateActive(GameManager stateController)
    {
    }


    public void UpdateActive(GameManager stateController)
    {
        
    }
}
public class GameDialogState : Singleton<GameDialogState>, IState<GameManager>
{
    public void Init()
    {
    }
    public void Enter(GameManager stateController)
    {
    }

    public void Exit(GameManager stateController)
    {
        Managers.Game.Player.SetStateIdle();
    }

    public void FixedUpdateActive(GameManager stateController)
    {
    }

    public void UpdateActive(GameManager stateController)
    {
    }
}

public class GameDebugModState : Singleton<GameDebugModState>, IState<GameManager>
{
    public void Init()
    {
    }
    public void Enter(GameManager stateController)
    {
        stateController.OnDebugModStateEnter();
    }
    public void Exit(GameManager stateController)
    {
        stateController.OnDebugModStateExit();
    }

    public void FixedUpdateActive(GameManager stateController)
    {
    }

    public void UpdateActive(GameManager stateController)
    {
    }
}
