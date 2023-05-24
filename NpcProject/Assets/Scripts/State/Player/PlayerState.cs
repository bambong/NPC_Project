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
        Managers.Sound.StopMoveSound();
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

public class PlayerWalk : Singleton<PlayerWalk>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.ChangeToRunSpeed(false);
        Managers.Sound.PlayMoveSound(Define.SOUND.WalkPlayer);
    }

    public void Exit(PlayerController stateController)
    {
        Managers.Sound.StopMoveSound();
    }

    public void FixedUpdateActive(PlayerController stateController)
    {
        stateController.PlayerWalkUpdate();
    }

    public void UpdateActive(PlayerController stateController)
    {
        stateController.InteractionInputCheck();
        stateController.DebugModEnterInputCheck();
    }
}

public class PlayerRun : Singleton<PlayerRun>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        Managers.Sound.PlayMoveSound(Define.SOUND.RunPlayer);
        stateController.ChangeToRunSpeed(true);
    }

    public void Exit(PlayerController stateController)
    {
        stateController.ChangeToRunSpeed(false);
        Managers.Sound.StopMoveSound();
    }

    public void FixedUpdateActive(PlayerController stateController)
    {
       
        stateController.PlayerRunUpdate();
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


public class PlayerInteraction : Singleton<PlayerInteraction>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.ClearMoveAnim();
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

public class PlayerDeath : Singleton<PlayerDeath>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        stateController.PlayDeathFeedback();
        Managers.Keyword.PlayerKeywordPanel.Close();
        Managers.Game.SetStateGameOver();
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