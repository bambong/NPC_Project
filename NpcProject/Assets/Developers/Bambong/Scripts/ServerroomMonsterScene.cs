using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ServerroomMonsterScene : BaseScene
{
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel tutorialSceneTalkData;

    [SerializeField]
    private Vector3 playerSpawnSpot;

    [SerializeField]
    private int monsterNeedCount = 5;
    [SerializeField]
    private int curMonsterDeathCount = 0;
    [SerializeField]
    private Define.Scene nextScene; 

    protected override void Init()
    {
        base.Init();
        var player = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(new CameraInfo(vircam, player.transform));
        Managers.Talk.LoadTalkData(tutorialSceneTalkData);
    }
    public void AddDeathCount() 
    {
        if (curMonsterDeathCount >= monsterNeedCount)
        {
            return;
        }
        ++curMonsterDeathCount;
        if( curMonsterDeathCount>= monsterNeedCount) 
        {
            Managers.Scene.LoadScene(nextScene);
        }
    }

    private void Start()
    {
        PlayBgm();
    }

    public override void Clear()
    {
    }
}