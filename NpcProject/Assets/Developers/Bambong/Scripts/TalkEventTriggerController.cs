
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkEventTriggerController : MonoBehaviour
{
    [SerializeField]
    private int talk;

    private bool isPlay = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (isPlay) 
        {
            return;
        }

        if (other.CompareTag("Player")) 
        {
            Managers.Talk.PlayCurrentSceneTalk(talk);
            isPlay = true;
        }
    }
}
