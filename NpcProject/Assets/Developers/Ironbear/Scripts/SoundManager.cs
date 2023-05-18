using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using System;

public class SoundManager
{    
    private Bus masterBus;
    private Bus bgmBus;
    private Bus sfxBus;

    public EventInstance sfxInstance;
    public StudioEventEmitter bgmEmitter;


    public void Init()
    {
        masterBus = RuntimeManager.GetBus("bus:/Master");
        bgmBus = RuntimeManager.GetBus("bus:/Master/BGM");
        sfxBus = RuntimeManager.GetBus("bus:/Master/SFX");

        CreateBGMEmitter();
    }

    private void CreateBGMEmitter()
    {
        GameObject bgmPrefab = Resources.Load<GameObject>("Prefabs/BGM");
        GameObject instance = UnityEngine.Object.Instantiate(bgmPrefab);
        UnityEngine.Object.DontDestroyOnLoad(instance);

        bgmEmitter = instance.GetComponent<StudioEventEmitter>();
    }

    public void LoadBank(Scene scene)
    {
        RuntimeManager.LoadBank(scene.name);
    }

    #region BGMControl
    public void PlayBGM() => bgmEmitter.Play();

    public void StopBGM(float fadeoutTime = 1.0f)
    {
        Managers.Scene.CurrentScene.StartCoroutine(Stop(fadeoutTime));
    }

    public void ChangeBGM(EventReference bgm, string param = null, float value = 0)
    {
        if (param == null)
        {
            bgmEmitter.ChangeEvent(bgm);
        }
        else
        {
            bgmEmitter.SetParameter(param, value);
        }
    }

    private IEnumerator Stop(float fadeOutDuration)
    {
        float startVolume;

        bgmEmitter.EventInstance.getParameterByName("Volume", out startVolume);
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeOutDuration)
        {
            float volume = Mathf.Lerp(1.0f, 0.0f, t);
            bgmEmitter.EventInstance.setParameterByName("Volume", volume);
            yield return null;
        }
        bgmEmitter.Stop();
    }
    #endregion


    public void PlaySFX(string eventpath)
    {
        sfxInstance = RuntimeManager.CreateInstance("event:/SFX/" + eventpath);
        sfxInstance.start();
    }

    public void SetPauseBGM(bool pause) => bgmEmitter.SetPause(pause);

    public void SetMasterVolume(float value) => masterBus.setVolume(value);

    public void SetBGMVolume(float value) => bgmBus.setVolume(value);

    public void SetSFXVolume(float value) => sfxBus.setVolume(value);

    public void Clear()
    {
        StopBGM();
    }

}
