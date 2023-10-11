using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayTalkEvent : GuIdBehaviour
{
    [SerializeField]
    private int[] talkId;
    [SerializeField]
    private UnityEvent startEvent;
    [SerializeField]
    private UnityEvent clearEvent;
    [SerializeField]
    private Transform endPosition;
    [SerializeField]
    private CharacterInfo left;
    [SerializeField]
    private CharacterInfo right;

    private Transform playertransform;

    private int count = 0;

    public void PlayDialogue()
    {        
        if (Managers.Data.IsClearEvent(guId))
        {
            return;
        }

        var talk = Managers.Talk.GetTalkEvent(talkId[count]);
        talk.OnStart(() => startEvent?.Invoke());

        Managers.Talk.PlayTalk(talk, left, right);

        if (count == talkId.Length - 1)
        {
            talk.OnComplete(() => clearEvent?.Invoke());
            talk.OnComplete(() => Managers.Data.ClearEvent(guId));
        }
        else
        {
            talk.OnComplete(() => count++);
            talk.OnComplete(() => PlayDialogue());
        }
    }

    public void GotoPositon()
    {
        
    }
}
