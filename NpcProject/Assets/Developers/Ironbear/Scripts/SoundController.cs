using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource bgmSource;
    [SerializeField]
    private AudioSource sfxSource;

    private string currentBgmName = null;
    private string area = null;

    public void GetAreaName(string areaName)
    {
        area = areaName;
    }

    public void BgmPlay(GameScene.BgmType[] bgm)
    {
        if (currentBgmName.Equals(area))
        {
            return;
        }

        for (int i = 0; i < bgm.Length; i++)
        {
            if (bgm[i].name.Equals(area))
            {
                bgmSource.clip = bgm[0].file;
                bgmSource.volume = bgm[0].volume;
                bgmSource.loop = true;
                bgmSource.Play();
                currentBgmName = area;
            }
        }
    }

    public void SfxPlay(AudioClip sfxClip = null)
    {
        sfxSource.clip = sfxClip;
        sfxSource.PlayOneShot(sfxClip);
    }
}
