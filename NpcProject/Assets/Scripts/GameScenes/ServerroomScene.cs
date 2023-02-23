using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerroomScene : BaseScene
{
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel tutorialSceneTalkData;
    
    [SerializeField]
    private Vector3 playerSpawnSpot;

    public override void Clear()
    {
      
    }

    protected override void Init()
    {
        base.Init();

        var player = Managers.Game.Spawn(Define.WorldObject.Player,"Player");
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(new CameraInfo(vircam,player.transform));
        Managers.Talk.LoadTalkData(tutorialSceneTalkData);

    }
}

