using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BgmData
{
    public string name;
    public AudioClip clip;
    public float volume;
    public bool load;
}
public struct SoundData
{
    public string name;
    public AudioClip clip;
    public float volume;
    public bool load;
}

public class SoundManager
{    

    private SoundController soundController;
    private BgmZone currentBgmZone;

    private Dictionary<int, BgmData> bgmDatas = new Dictionary<int, BgmData>();
    private Dictionary<int, SoundData> sfxDatas = new Dictionary<int, SoundData>();

    private SoundExcel soundExcel;
    public void Init()
    {
        var soundPrefab = Managers.Resource.Instantiate("SoundSource");
        Object.DontDestroyOnLoad(soundPrefab.gameObject);
        soundPrefab.name = "@SoundSource";
        soundController = soundPrefab.GetComponent<SoundController>();
        soundExcel = Resources.Load<SoundExcel>($"Data/SoundData/SoundExcel");
        LoadSoundData();
    }

    private SoundData ReadSfxData(SoundEvent eventData) 
    {
        SoundData data = new SoundData();
        data.clip = Resources.Load<AudioClip>($"Sounds/SFX/{eventData.Name}");
        data.volume = eventData.Vol;
        return data;
    }

    public void LoadSoundData()
    {

        for(int i = 0; i < soundExcel.datas.Count; i++)
        {
            var data = soundExcel.datas[i];

            if(data.soundType == "SFX")
            {
                SoundData soundData = new SoundData();
                soundData.name = data.soundName;
                soundData.volume = data.soundVol;
                soundData.load = false;
                sfxDatas.Add(data.soundID, soundData);
            }
            else
            {
                BgmData bgmData = new BgmData();
                bgmData.name = data.soundName;
                bgmData.volume = data.soundVol;
                bgmData.load = false;
                bgmDatas.Add(data.soundID, bgmData);
            }
        }
    }

    public void AskBgmPlay(int id)
    {
        if(bgmDatas[id].load == false)
        {
            Debug.Log("Load Sound ID: " + id);
            BgmData bgmData = new BgmData();
            bgmData.clip = Resources.Load<AudioClip>($"Sounds/BGM/{bgmDatas[id].name}");
            bgmData.name = bgmDatas[id].name;
            bgmData.volume = bgmDatas[id].volume;
            bgmData.load = true;

            bgmDatas[id] = bgmData;
        }
        soundController.BgmPlay(bgmDatas[id].clip, bgmDatas[id].volume);
    }

    public void AskSfxPlay(int id)
    {
        if(sfxDatas[id].load == false)
        {
            Debug.Log("Load Sound ID:" + id);
            SoundData soundData = new SoundData();
            soundData.clip = Resources.Load<AudioClip>($"Sounds/SFX/{sfxDatas[id].name}");
            soundData.name = sfxDatas[id].name;
            soundData.volume = sfxDatas[id].volume;
            soundData.load = true;

            sfxDatas[id] = soundData;
        }
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
