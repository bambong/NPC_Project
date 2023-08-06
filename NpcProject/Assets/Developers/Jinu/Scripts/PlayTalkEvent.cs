using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTalkEvent : MonoBehaviour
{
    public void PlayDialogue(int id)
    {
        var talk = Managers.Talk.GetTalkEvent(id);
        Managers.Talk.PlayTalk(talk);
    }
}
