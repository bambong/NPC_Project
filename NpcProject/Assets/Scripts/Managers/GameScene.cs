using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameScene : BaseScene
{
    [SerializeField]
    private Vector3 playerSpawnSpot;
    [SerializeField]
    private CinemachineVirtualCamera vircam;

    [SerializeField]
    private Talk startTalk;

    protected override void Init() 
    {
        base.Init();

        var player  = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(vircam,player.transform);

        Invoke("StartTalk", 2f);    

        var rotatekeyword  = Managers.UI.MakeSubItem<KeywordController>(null,"RotateKeyword");
        Managers.Keyword.AddKeywordToPlayer(rotatekeyword);
        rotatekeyword.transform.localScale = Vector3.one;

        var zmovekeyword = Managers.UI.MakeSubItem<KeywordController>(null,"ZMoveKeyword");
        Managers.Keyword.AddKeywordToPlayer(zmovekeyword);
        zmovekeyword.transform.localScale = Vector3.one;
       
        var xmovekeyword = Managers.UI.MakeSubItem<KeywordController>(null,"XMoveKeyword");
        Managers.Keyword.AddKeywordToPlayer(xmovekeyword);
        xmovekeyword.transform.localScale = Vector3.one;
    }
    public void StartTalk() 
    {
        Managers.Talk.EnterTalk(startTalk, null);

    }
   
    public override void Clear()
    {
        
    }
}
