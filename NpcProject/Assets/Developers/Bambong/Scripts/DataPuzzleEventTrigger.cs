using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPuzzleEventTrigger : EventTrigger
{
    [SerializeField]
    private int progress = 0;
    [SerializeField]
    private MiniGameLevelData miniGameLevel;
    public override void OnEventTrigger(Collider other)
    {
        if (!Managers.Data.isAvaildProgress(progress)) 
        {
            return;
        }
        Managers.Game.Player.SetstateStop();
        Managers.Scene.LoadScene(Define.Scene.DataPuzzle);
        base.OnEventTrigger(other);
    }
}
