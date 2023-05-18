using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel gamaSceneTalkData;

    protected override void Init()
    {
        base.Init();

        var player = Managers.Game.Spawn(Define.WorldObject.Player,"Player");
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(new CameraInfo(vircam,player.transform));

        Managers.Talk.LoadTalkData(gamaSceneTalkData);
    }


    public override void Clear()
    {
    }
}
