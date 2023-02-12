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

    private string currentBgmName = null;
    private string area = null;

    public void GetAreaName(string areaName)
    {
        area = areaName;
    }

    public void BgmPlay(BgmType[] bgm)
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
                bgmSource.clip = bgm[i].file;
                bgmSource.volume = bgm[i].volume;
                bgmSource.loop = true;
                bgmSource.Play();
                currentBgmName = name;
            }
        }
    }

    public void SfxPlay(AudioClip sfxClip = null)
    {
        sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sfx")[0];
        sfxSource.clip = sfxClip;
        sfxSource.PlayOneShot(sfxClip);
    }
}
