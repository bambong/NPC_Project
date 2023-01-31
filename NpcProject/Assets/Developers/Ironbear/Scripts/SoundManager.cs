using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private SoundController soundController;
    private BgmZone currentBgmZone;

    

    public void Init()
    {
        var soundPrefab = Managers.Resource.Instantiate("SoundSource");
        soundPrefab.name = "@SoundSource";
        soundController = soundPrefab.GetComponent<SoundController>();
    }

    public void AskAreaName(string areaName)
    {
        soundController.GetAreaName(areaName);
    }
    
    public void AskBgmPlay(GameScene.BgmType[] bgm)
    {
        soundController.BgmPlay(bgm); 
    }

    public void AskSfxPlay(AudioClip sfxClip)
    {
        soundController.SfxPlay(sfxClip);
    }

    public void CheckBgmZone(BgmZone zone)
    {
        currentBgmZone = zone;
    }
}
