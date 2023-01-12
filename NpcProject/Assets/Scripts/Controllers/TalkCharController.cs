using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCharController : MonoBehaviour, IInteraction
{
    [SerializeField]
    private int talk;

    [SerializeField]
    private int repeatTalk;

    public GameObject Go => gameObject;


    private bool isPlay = false;


    public void OnInteraction()
    {
        if (isPlay) 
        {

            Managers.Talk.PlayCurrentSceneTalk(repeatTalk);
            return;
        }
        isPlay = true;
        Managers.Talk.PlayCurrentSceneTalk(talk);   
    }
}
