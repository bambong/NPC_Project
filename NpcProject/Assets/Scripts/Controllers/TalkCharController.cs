using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCharController : MonoBehaviour, IInteraction
{
    [SerializeField]
    private Talk talk;
    
    public GameObject Go => gameObject;



    public void OnInteraction()
    {
        Managers.Talk.EnterTalk(talk,null);   
    }
}
