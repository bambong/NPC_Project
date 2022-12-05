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

public class GameManager
{
    private GameStateController gameStateController;
    private PlayerController player;
    public PlayerController Player { get => player; }


    public void Init()
    {
        gameStateController = new GameStateController(this);
    }

    #region SetState
    public void SetStateNormal() => gameStateController.ChangeState(GameNormalState.Instance);
    public void SetStateDialog() => gameStateController.ChangeState(GameDialogState.Instance);
    #endregion
    
    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Player:
                player = go.GetComponent<PlayerController>();
                break;
        }
        return go;
    }


    public void Clear() 
    {
        gameStateController.ChangeState(GameNormalState.Instance);
        player = null;
    }
    
}
