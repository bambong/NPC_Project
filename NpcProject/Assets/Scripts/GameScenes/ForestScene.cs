using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ForestScene : BaseScene
{
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel tutorialSceneTalkData;
    
    [SerializeField]
    private Vector3 playerSpawnSpot;

    [SerializeField]
    private string playerType = "Player";

    public override void Clear()
    {
        
    }

    protected override void Init()
    {
        base.Init();
        var player = Managers.Game.Spawn(Define.WorldObject.Player, playerType);
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(new CameraInfo(vircam, player.transform));
        Managers.Data.LoadGame(SceneManager.GetActiveScene().name);
        Managers.Talk.LoadTalkData(tutorialSceneTalkData);

       // Managers.UI.MakeSceneUI()
    }
    private void Start()
    {
        PlayBgm();
    }
}