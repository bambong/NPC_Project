using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ServerroomMonsterScene : BaseScene
{
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel tutorialSceneTalkData;

    [SerializeField]
    private int monsterNeedCount = 5;
    [SerializeField]
    private int curMonsterDeathCount = 0;
    [SerializeField]
    private Define.Scene nextScene;

    [SerializeField]
    private UnityEvent onClearScene;


    protected override void Init()
    {
        base.Init();
        var player = Managers.Game.Spawn(Define.WorldObject.Player, "Player_Real");
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
            onClearScene?.Invoke();
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