using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private SoundController soundController;

    public void Init()
    {
        Managers.Resource.Instantiate("SoundSource").name = "@SoundSource";
    }
    
    public void AskBgmPlay(AudioClip bgmClip)
    {
        soundController.BgmPlay(bgmClip);
    }

    public void AskSfxPlay(AudioClip sfxClip)
    {
        soundController.SfxPlay(sfxClip);
    }
}
