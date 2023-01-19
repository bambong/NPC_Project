using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public GameObject Go => throw new System.NotImplementedException();
    
    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip file;
        public float volume;
    }

    [SerializeField]
    private BgmType[] bgm;
    [SerializeField]
    private AudioSource bgmAudiosource;

    private void Awake()
    {
        if (bgm.Length > 0)
        {
            Managers.Sound.BgmPlay(bgmAudiosource, bgm[0].name, bgm[0].file, bgm[0].volume);
        }
    }
}
