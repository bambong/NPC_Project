using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public GameObject Go => throw new System.NotImplementedException();

    [SerializeField]
    private AudioClip[] bgmList;
    [SerializeField]
    private AudioSource bgmAudiosource;


    private void Awake()
    {
            Managers.Sound.BgmPlay(bgmAudiosource, bgmList[0]);   
    }
}
