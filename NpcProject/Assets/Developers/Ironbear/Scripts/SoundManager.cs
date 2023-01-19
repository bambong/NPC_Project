using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private GameObject go;
    private string currentArea;
    private string currentBgm = "";

    public void Init()
    {
        Managers.Resource.Instantiate("Sound").name = "@Sound";
        go = GameObject.Find("@Sound");
    }
    
    public void areaCheck(string name)
    {
        currentArea = name;
        Debug.Log(name);
    }

    public void BgmPlay(AudioSource bgmSource, string bgmName, AudioClip bgmFile, float volume)
    {
        if (currentBgm.Equals(bgmName))
        {
            return;
        }
        
        bgmSource.clip = bgmFile;
        bgmSource.loop = true;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    public void SfxPlay(AudioClip sfxfile = null)
    {
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.PlayOneShot(sfxfile);
    }
}
