using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEventTrigger : EventTrigger
{
    [SerializeField]
    private Define.Scene transitionScene;
    [SerializeField]
    private Define.SOUND soundname;
    public override void OnEventTrigger(Collider other)
    {
        Managers.Game.Player.SetstateStop();
        Managers.Sound.PlaySFX(soundname);
        Managers.Scene.LoadScene(transitionScene);
        base.OnEventTrigger(other);
    }
}
