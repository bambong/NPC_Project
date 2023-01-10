using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    public void Init()
    {
    }

    public void BgmPlay(AudioSource bgmSound, AudioClip bgmfile)
    {
        bgmSound.clip = bgmfile;
        bgmSound.loop = true;
        bgmSound.volume = 0.05f;
        bgmSound.Play();
    }

    public void SfxPlay(string sfxName, AudioClip sfxfile = null)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = sfxfile;
        audioSource.Play();
    }
}
