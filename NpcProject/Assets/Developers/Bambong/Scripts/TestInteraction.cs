using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestInteraction : KeywordEntity, IInteraction 
{
    [SerializeField]
    public Talk talkData;
    [SerializeField]
    public CinemachineVirtualCamera virCam;

    public GameObject Go => gameObject;

    public void OnInteraction()
    {
        Managers.Talk.EnterTalk(talkData, virCam);
    }


}
