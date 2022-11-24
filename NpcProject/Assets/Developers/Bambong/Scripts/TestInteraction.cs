using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteraction : Interaction
{
    [SerializeField]
    public Talk talkData;
    public override void OnInteraction()
    {
        TalkManager.Instance.EnterTalk(talkData);
    }
}
