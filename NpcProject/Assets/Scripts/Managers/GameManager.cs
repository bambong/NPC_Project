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
    private KeyMappingController key = new KeyMappingController();
    private PlayerController player;
    private Vector3 prevGravity;
    private RetryPanelController retryPanel;
    private bool isDebugMod = false;
    public PlayerController Player { get => player; }
    public KeyMappingController Key { get => key;}
    public IState<GameManager> CurState { get => gameStateController.CurState; }
    public bool IsDebugMod { get => isDebugMod; }
    public RetryPanelController RetryPanel { get => retryPanel; }

    private float DEBUG_TIME_SCALE = 0.2f;



    public void Init()
    {
        gameStateController = new GameStateController(this);
        retryPanel = Managers.UI.MakeSceneUI<RetryPanelController>(null, "RetryPanelUI");
        SetStateNormal();

    }
    public void OnSceneLoaded()
    {
        retryPanel = Managers.UI.MakeSceneUI<RetryPanelController>(null, "RetryPanelUI");
        SetStateNormal();
        isDebugMod = false;
    }
    #region SetState
    public void SetStateNormal() => gameStateController.ChangeState(GameNormalState.Instance);
    public void SetStateEvent() => gameStateController.ChangeState(GameEventState.Instance);
    public void SetEnableDebugMod() 
    {
        isDebugMod = true;
        OnDebugModStateEnter();
    }
    public void SetDisableDebugMod()
    {
        isDebugMod = false;
        OnDebugModStateExit();
    }
    public void SetStateGameOver() => gameStateController.ChangeState(GameOverState.Instance);
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
       
        if (isDebugMod) 
        {
            SetDisableDebugMod();
        }
    }
    #region StateEnter
    public void OnDebugModStateEnter()
    {
        Managers.Time.SetTimeScale(TIME_TYPE.NONE_PLAYER, DEBUG_TIME_SCALE);
        prevGravity = Physics.gravity;
        Physics.gravity = prevGravity * DEBUG_TIME_SCALE;

    }
    #endregion StateEnter
    #region StateExit
    public void OnDebugModStateExit()
    {
        Physics.gravity = prevGravity;
        Managers.Time.SetTimeScale(TIME_TYPE.NONE_PLAYER, 1);
    }
    #endregion StateExit
}
