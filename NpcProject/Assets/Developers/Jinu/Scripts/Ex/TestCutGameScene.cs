using Cinemachine;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TestCutGameScene: BaseScene
{
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel tutorialSceneTalkData;

    [SerializeField]
    private Vector3 playerSpawnSpot;

    [SerializeField]
    private PlayableDirector playableDirector;

    public CutSceneEvent cutScene;

    [SerializeField]
    private EventReference bgm;

    public override void Clear()
    {

    }

    protected override void Init()
    {
        base.Init();

        var player = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(new CameraInfo(vircam, player.transform));
        Managers.Talk.LoadTalkData(tutorialSceneTalkData);

        cutScene = new CutSceneEvent(playableDirector);
        cutScene.Play();        
        
    }

    private void Start()
    {
        Managers.Sound.SetBGMVolume(0.1f);
        Managers.Sound.SetSFXVolume(0.5f);

        Managers.Sound.ChangeBGM(bgm);
        Managers.Sound.BGMControl(Define.BGM.Start);
        //Managers.Sound.AskBgmPlay(20100);
        //Managers.Sound.AskBgmPlay(20100);
    }
}