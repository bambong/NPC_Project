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
    protected override void Init() 
    {
        base.Init();

        var player  = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(vircam,player.transform);
        var rotatekeyword  = Managers.UI.MakeSubItem<KeywordController>(null,"RotateKeyword");
        Managers.Keyword.AddKeywordToPlayer(rotatekeyword);
        rotatekeyword.transform.localScale = Vector3.one;
    }

    public override void Clear()
    {
        
    }
}
