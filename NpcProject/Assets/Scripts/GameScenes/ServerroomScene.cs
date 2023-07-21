using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using FMODUnity;
using Michsky.UI.MTP;

public class ServerroomScene : BaseScene
{
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel tutorialSceneTalkData;
    

    [SerializeField]
    private string playerType = "Player_Real";

    public override void Clear()
    {
        
    }

    protected override void Init()
    {
        base.Init();
        var player = Managers.Game.Spawn(Define.WorldObject.Player, playerType);
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(new CameraInfo(vircam, player.transform));
       
        Managers.Talk.LoadTalkData(tutorialSceneTalkData);

       // Managers.UI.MakeSceneUI()
    }
    private void Start()
    {
        PlayBgm();
    }
}