using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameEventTrigger : EventTrigger
{
    public override void OnEventTrigger(Collider other)
    {
        base.OnEventTrigger(other);
        Managers.Data.SaveGame();
    }

}
