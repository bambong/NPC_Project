using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource bgmSource;
    [SerializeField]
    private AudioSource sfxSource;

    public void BgmPlay(AudioClip bgmClip = null, float volume = 0)
    {        
        bgmSource.clip = bgmClip;
        bgmSource.volume = volume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SfxPlay(AudioClip sfxClip = null)
    {
        sfxSource.clip = sfxClip;
        sfxSource.PlayOneShot(sfxClip);
    }
}
