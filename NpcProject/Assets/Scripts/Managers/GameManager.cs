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
    private PausePanelController pausePanel;
    private BasePanelController blurPanel;

    private DestinationPanelController destinationPanel;
    private float prevTimeScale = 1f;
    private bool isDebugMod = false;
    public PlayerController Player { get => player; }
    public KeyMappingController Key { get => key;}
    public IState<GameManager> CurState { get => gameStateController.CurState; }
    public bool IsDebugMod { get => isDebugMod; }
    public bool IsPause { get => gameStateController.CurState == GamePauseState.Instance; }
    public RetryPanelController RetryPanel { get => retryPanel; }
    public PausePanelController PausePanel { get => pausePanel; }
    public BasePanelController BlurPanel { get => blurPanel; }
    public DestinationPanelController DestinationPanel { get => destinationPanel; }

    private float DEBUG_TIME_SCALE = 0.2f;


    public void Init()
    {
        gameStateController = new GameStateController(this);
        retryPanel = Managers.UI.MakeSceneUI<RetryPanelController>(null, "RetryPanelUI");
        pausePanel = Managers.UI.MakeSceneUI<PausePanelController>(null, "PausePanel");
        blurPanel = Managers.UI.MakeCameraSpaceUI<BasePanelController>(1,null,"BlurPanel");
        destinationPanel = Managers.UI.MakeSceneUI<DestinationPanelController>(null, "DestinationPanel");
        prevGravity = Physics.gravity;
        SetStateNormal();

    }
    public void OpenDestination(string text) 
    {
        destinationPanel.Open(text);
    } 
    public void OnSceneLoaded()
    {
        retryPanel = Managers.UI.MakeSceneUI<RetryPanelController>(null, "RetryPanelUI");
        blurPanel = Managers.UI.MakeCameraSpaceUI<BasePanelController>(1, null, "BlurPanel");
        SetStateNormal();
    

        isDebugMod = false;
    }
    #region SetState
    public void SetStateNormal() => gameStateController.ChangeState(GameNormalState.Instance);
    public void SetStateEvent() => gameStateController.ChangeState(GameEventState.Instance);
    public void SetStatePause()
    {
        if (Managers.Scene.IsTransitioning)
        {
            return;
        }
        gameStateController.ChangeState(GamePauseState.Instance);
    }
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
        Physics.gravity = prevGravity;
        Managers.Time.SetTimeScale(TIME_TYPE.NONE_PLAYER, 1);
        destinationPanel.Close();
    }

    #region StateEnter
    public void OnDebugModStateEnter()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DebugModeEnter);
        Managers.Time.SetTimeScale(TIME_TYPE.NONE_PLAYER, DEBUG_TIME_SCALE);
        prevGravity = Physics.gravity;
        Physics.gravity = prevGravity * DEBUG_TIME_SCALE;
    }
    public void OnPauseStateEnter()
    {
        pausePanel.Open();
        blurPanel.Open();
     
       
        player.SetStatePause();
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0;

    }
    #endregion StateEnter
    #region StateExit
    public void OnDebugModStateExit()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DebugModeExit);
        Physics.gravity = prevGravity;
        Managers.Time.SetTimeScale(TIME_TYPE.NONE_PLAYER, 1);
    }
    public void OnPauseStateExit()
    {
        player.RevertState();
        pausePanel.Close();
        blurPanel.Close();
        Time.timeScale = prevTimeScale;
    }
    #endregion StateExit
}
