using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCharController : MonoBehaviour, IInteraction
{
    [SerializeField]
    private Talk talk;

    [SerializeField]
    private Talk repeatTalk;

    public GameObject Go => gameObject;


    private bool isPlay = false;


    public void OnInteraction()
    {
        if (isPlay) 
        {

            Managers.Talk.EnterTalk(repeatTalk, null);
            return;
        }
        isPlay = true;
        //Managers.Scene.LoadScene(Define.Scene.Clear);
        Managers.Talk.EnterTalk(talk,null);   
    }
}
