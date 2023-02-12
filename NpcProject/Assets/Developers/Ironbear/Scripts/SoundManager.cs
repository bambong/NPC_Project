using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BgmType
{
    public string name;
    public AudioClip file;
    public float volume;
}


public class SoundManager
{    

    private SoundController soundController;
    private BgmZone currentBgmZone;

    private Dictionary<string, BgmType> bgmDatas = new Dictionary<string, BgmType>();
    

    public void Init()
    {
        var soundPrefab = Managers.Resource.Instantiate("SoundSource");
        soundPrefab.name = "@SoundSource";
        soundController = soundPrefab.GetComponent<SoundController>();
    }

    public void LoadBgmDatas(BgmType[] bgmTypes)
    {
       for(int i = 0; i < bgmTypes.Length; ++i)
        {
            bgmDatas.Add(bgmTypes[i].name, bgmTypes[i]);
        }
    }

    
    public void AskBgmPlay(BgmType[] bgm)
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

    public void Clear()
    {
        bgmDatas.Clear();
    }
}
