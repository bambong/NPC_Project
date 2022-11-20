using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : StateController<GameSceneManager>
{
    public GameStateController(GameSceneManager owner) : base(owner)
    {
        Init();
    }
    public void Init()
    {
        curState = .Instance;
    }

}

public class GameManager : Singleton<GameManager>
{
    
}
