using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : StateController<GameManager>
{
    public GameStateController(GameManager owner) : base(owner)
    {
        Init();
    }
    public void Init()
    {
        curState = GameNormalState.Instance;
    }

}

public class GameManager : Singleton<GameManager>,IInit
{
    GameStateController gameStateController;

    public void Init()
    {
        gameStateController = new GameStateController(Instance);
    }

    #region SetState
    public void SetStateNormal() => gameStateController.ChangeState(GameNormalState.Instance);
    public void SetStateDialog() => gameStateController.ChangeState(GameDialogState.Instance);
    #endregion
}
