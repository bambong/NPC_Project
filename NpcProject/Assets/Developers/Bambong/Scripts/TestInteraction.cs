using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestInteraction : Interaction
{
    [SerializeField]
    public Talk talkData;
    [SerializeField]
    public CinemachineVirtualCamera virCam;

    public override void OnInteraction()
    {
        TalkManager.Instance.EnterTalk(talkData, virCam);
    }
}
