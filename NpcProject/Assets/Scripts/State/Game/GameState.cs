using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseState : Singleton<GamePauseState>, IState<GameManager>
{
    public void Init()
    {
    }

    public void Enter(GameManager stateController)
    {
        stateController.OnPauseStateEnter();
    }

    public void Exit(GameManager stateController)
    {
        stateController.OnPauseStateExit();
    }

    public void FixedUpdateActive(GameManager stateController)
    {
    }


    public void UpdateActive(GameManager stateController)
    {

    }
}

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
public class GameEventState : Singleton<GameEventState>, IState<GameManager>
{
    public void Init()
    {
    }
    public void Enter(GameManager stateController)
    {
        Managers.Game.Player.SetstateStop();
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

public class GameOverState : Singleton<GameOverState>, IState<GameManager>
{
    public void Init()
    {
    }
    public void Enter(GameManager stateController)
    {
        stateController.RetryPanel.OpenRetryPanel();
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

