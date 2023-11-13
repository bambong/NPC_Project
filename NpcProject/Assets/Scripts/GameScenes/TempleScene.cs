using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TempleScene : BaseScene
{
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel tutorialSceneTalkData;

    [SerializeField]
    private string playerType = "Player";

    [SerializeField]
    private GameObject wall;

    [SerializeField]
    private string hideEventGuid;

    [SerializeField]
    private Define.Scene scene;

    public override void Clear()
    {

    }

    protected override void Init()
    {
        base.Init();
        var player = Managers.Game.Spawn(Define.WorldObject.Player,playerType);
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(new CameraInfo(vircam,player.transform));
        Managers.Data.LoadGame(SceneManager.GetActiveScene().name);
        Managers.Talk.LoadTalkData(tutorialSceneTalkData);

        // Managers.UI.MakeSceneUI()
    }
    private void Start()
    {
        PlayBgm();
        if(Managers.Data.IsClearEvent(hideEventGuid))
        {
            wall.SetActive(true);
        }
    }

    public void NextScene()
    {
        Managers.Game.Player.SetstateStop();
        Managers.Scene.LoadScene(scene);
    }
}