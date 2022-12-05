using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    private Vector3 playerSpawnSpot;

    protected override void Init() 
    {
       var player  = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
       player.transform.position = playerSpawnSpot;
       Camera.main.GetComponent<CameraController>().SetTarger(player.transform);
    }

    public override void Clear()
    {
        
    }
}
