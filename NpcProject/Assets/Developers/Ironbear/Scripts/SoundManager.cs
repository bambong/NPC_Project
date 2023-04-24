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
public struct SoundData
{
    public AudioClip clip;
    public float volume;
}

public class SoundManager
{    

    private SoundController soundController;
    private BgmZone currentBgmZone;

    private Dictionary<string, BgmType> bgmDatas = new Dictionary<string, BgmType>();
    private Dictionary<int, SoundData> sfxDatas = new Dictionary<int, SoundData>();

    public void Init()
    {
        var soundPrefab = Managers.Resource.Instantiate("SoundSource");
        Object.DontDestroyOnLoad(soundPrefab.gameObject);
        soundPrefab.name = "@SoundSource";
        soundController = soundPrefab.GetComponent<SoundController>();
        
        //var lists = Resources.Load<SoundLists>("SoundLists");
        //for (int i = 0; i < lists.SoundEvents.Count; ++i) 
        //{
        //    sfxDatas.Add(lists.SoundEvents[i].Id, ReadSfxData(lists.SoundEvents[i]));
        //}
    }
    private SoundData ReadSfxData(SoundEvent eventData) 
    {
        SoundData data = new SoundData();
        data.clip = Resources.Load<AudioClip>($"Sounds/SFX/{eventData.Name}");
        data.volume = eventData.Vol;
        return data;
    }

    public void LoadSoundData(SoundExcel soundExcel)
    {
        for(int i = 0; i < soundExcel.datas.Count; i++)
        {
            var data = soundExcel.datas[i];

            if(data.soundType == "SFX")
            {
                SoundData soundData = new SoundData();
                soundData.clip = Resources.Load<AudioClip>($"Sounds/SFX/{data.soundName}");
                soundData.volume = data.soundVol;

                sfxDatas.Add(data.soundID, soundData);
            }
            else
            {
                //BgmType bgmType = new BgmType();
                //bgmType.name = data.soundName;
                //bgmType.file = data.
            }
        }
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
    public void AskSfxPlay(int id)
    {
        soundController.SfxPlay(sfxDatas[id].clip, sfxDatas[id].volume);
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
