using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private SoundController soundController;

    public void Init()
    {
        var soundPrefab = Managers.Resource.Instantiate("SoundSource");
        soundPrefab.name = "@SoundSource";
        soundController = soundPrefab.GetComponent<SoundController>();
    }
    
    public void AskBgmPlay(AudioClip bgmClip, float volume)
    {
        soundController.BgmPlay(bgmClip, volume);
    }

    public void AskSfxPlay(AudioClip sfxClip)
    {
        soundController.SfxPlay(sfxClip);
    }
}
