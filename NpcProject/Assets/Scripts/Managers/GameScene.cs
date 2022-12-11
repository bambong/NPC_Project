using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameScene : BaseScene
{
    [SerializeField]
    private Vector3 playerSpawnSpot;
    [SerializeField]
    private CinemachineVirtualCamera vercam;
    protected override void Init() 
    {
        base.Init();

        var player  = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        player.transform.position = playerSpawnSpot;
        Managers.Camera.InitCamera(vercam,player.transform);
    }

    public override void Clear()
    {
        
    }
}
