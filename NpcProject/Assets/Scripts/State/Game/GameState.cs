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
       
        stateController.Player.SetStateDebugMod();
        if (Managers.Game.Player.CurKeywordInteraction != null)
        {
            Managers.Game.Player.CurKeywordInteraction.CloseWorldSlotUI();
        }
    }
    public void Exit(GameManager stateController)
    {
        //Managers.Keyword.ExitDebugMod();
    }

    public void FixedUpdateActive(GameManager stateController)
    {
    }

    public void UpdateActive(GameManager stateController)
    {
    }
}
public class GameKeywordModState : Singleton<GameKeywordModState>, IState<GameManager>
{
    public void Init()
    {
    }
    public void Enter(GameManager stateController)
    {
        Time.timeScale = 0;
        stateController.Player.SetStatekeywordMod();
    }

    public void Exit(GameManager stateController)
    {
        Time.timeScale = 1;
    }

    public void FixedUpdateActive(GameManager stateController)
    {
    }

    public void UpdateActive(GameManager stateController)
    {
    }
}