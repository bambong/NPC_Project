using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource bgmSource;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioMixer audioMixer;

    AudioMixerSnapshot defaultSnapshot;

    private string currentBgmName = "";
    private string currentSfxName = "";
    private string area = null;
    
    private int sfxCount = -2;

    public void GetAreaName(string areaName)
    {
        area = areaName;
    }

    public void BgmPlay(BgmData[] bgm)
    {
        if (currentBgmName.Equals(name))
        {
            return;
        }

        for (int i = 0; i < bgm.Length; ++i)
        {
            if (bgm[i].name.Equals(name))
            {
                bgmSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Bgm")[0];
                bgmSource.clip = bgm[i].clip;
                bgmSource.volume = bgm[i].volume;
                bgmSource.loop = true;
                bgmSource.Play();
                currentBgmName = name;
            }
        }
    }

    public void BgmPlay(AudioClip bgmClip, float vol)
    {
        if(currentBgmName.Equals(bgmClip.name))
        {
            Debug.Log("Same BGM is already running");
            return;
        }
        else
        {
            bgmSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Bgm")[0];
            bgmSource.clip = bgmClip;
            bgmSource.volume = vol;
            bgmSource.loop = true;
            bgmSource.Play();
            currentBgmName = bgmClip.name;
        }
    }

    public void SfxPlay(AudioClip sfxClip = null , float vol = 1)
    {
        if (sfxSource.GetComponent<AudioSource>().isPlaying && currentSfxName == sfxClip.name)
        {
            sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sfx")[0];
            float curVol = Mathf.Pow(2, sfxCount);
            Debug.Log(curVol);

            sfxSource.PlayOneShot(sfxClip, Mathf.Pow(2, sfxCount));
            sfxCount--;
        }
        else
        {
            sfxCount = -5;
            sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sfx")[0];
            sfxSource.PlayOneShot(sfxClip, vol);
            currentSfxName = sfxClip.name;
        }
    }
}
