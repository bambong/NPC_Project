using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : GameObjectSingletonDestroy<GameSceneManager>, IInit
{
    [SerializeField]
    private PlayerController player;

    public PlayerController Player { get => player; }
    
    public void Init()
    {

    }
}
