using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalEventTrigger : EventTrigger
{
    [SerializeField]
    private Define.Scene transitionScene;
    public override void OnEventTrigger(Collider other)
    {
        Managers.Game.Player.SetstateStop();
        Managers.Scene.LoadScene(transitionScene);
        base.OnEventTrigger(other);
    }
}
