using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameScene : BaseScene
{
    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip file;
        public float volume;
    }

    [SerializeField]
    private BgmType[] bgm;

    [SerializeField]
    private Vector3 playerSpawnSpot;
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private DialogueExcel gamaSceneTalkData;


    [SerializeField]
    private int startTalk;

    protected override void Init() 
    {
        base.Init();

        var player  = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(new CameraInfo(vircam,player.transform));

        StartBgm();
        Invoke("StartTalk", 2f);
        Managers.Talk.LoadTalkData(gamaSceneTalkData);

    }
    public void StartTalk()
    {
        var talk = Managers.Talk.GetTalkEvent(startTalk);
        talk.OnStart(() => Managers.Game.Player.SetstateStop());
        talk.OnComplete(() => Managers.Game.Player.SetStateIdle());
        Managers.Talk.PlayCurrentSceneTalk(startTalk);
    }
    
    public void StartBgm()
    {
        Managers.Sound.AskBgmPlay(bgm[0].file, bgm[0].volume);
    }

    public override void Clear()
    {
        
    }
}
