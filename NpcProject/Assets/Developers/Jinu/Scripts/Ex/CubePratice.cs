using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePratice : MonoBehaviour
{
    [SerializeField]
    float setScale = 1.0f;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Player")
        {
            //Managers.Sound.BGMControl(Define.BGM.Pause);
            Time.timeScale = setScale;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Managers.Sound.BGMControl(Define.BGM.ReStart);
        }
    }

}
