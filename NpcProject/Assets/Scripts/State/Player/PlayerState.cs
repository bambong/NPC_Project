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
        Managers.Sound.StopSFX();
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
        Managers.Sound.PlaySFX(Define.SOUND.WalkPlayer);
    }

    public void Exit(PlayerController stateController)
    {
        Managers.Sound.StopSFX();
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

public class PlayerRun : Singleton<PlayerRun>, IState<PlayerController>
{
    public void Init()
    {
    }
    public void Enter(PlayerController stateController)
    {
        Managers.Sound.PlaySFX(Define.SOUND.RunPlayer);
    }

    public void Exit(PlayerController stateController)
    {
        Managers.Sound.StopSFX();
    }

    public void FixedUpdateActive(PlayerController stateController)
    {
        stateController.ChangeToRunSpeed(true);
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
        //Managers.Sound.AskSfxPlay(20008);
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