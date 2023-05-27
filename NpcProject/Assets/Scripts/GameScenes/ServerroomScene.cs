using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using FMODUnity;

public class ServerroomScene : BaseScene
{
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel tutorialSceneTalkData;
    
    [SerializeField]
    private Vector3 playerSpawnSpot;
   
    [SerializeField]
    private int progress = 1;

    [SerializeField]
    private EventReference bgm;

    public override void Clear()
    {
      // Managers.Data.SaveGame(SceneManager.GetActiveScene().name);
    }

    protected override void Init()
    {
        base.Init();
        var player = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(new CameraInfo(vircam, player.transform));
        Managers.Talk.LoadTalkData(tutorialSceneTalkData);
        Managers.Data.LoadGame(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        Managers.Sound.ChangeBGM(bgm);
        Managers.Sound.BGMControl(Define.BGM.Start);
    }
}