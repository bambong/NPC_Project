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

    //public EventInstance bgmInstance;
    public EventInstance sfxInstance;

    public StudioEventEmitter bgmEmitter;
    private int bgmTime;

    public void Init()
    {
        masterBus = RuntimeManager.GetBus("bus:/Master");
        bgmBus = RuntimeManager.GetBus("bus:/Master/BGM");
        sfxBus = RuntimeManager.GetBus("bus:/Master/SFX");

        CreateBGMEmitter();        

        SceneManager.activeSceneChanged += SceneManagerOnactiveSceneChanged;
        SceneManagerOnactiveSceneChanged(SceneManager.GetSceneByName("NULL"), SceneManager.GetActiveScene());
    }

    private void CreateBGMEmitter()
    {
        GameObject bgmPrefab = Resources.Load<GameObject>("Prefabs/BGM");
        GameObject instance = UnityEngine.Object.Instantiate(bgmPrefab);
        UnityEngine.Object.DontDestroyOnLoad(instance);

        bgmEmitter = instance.GetComponent<StudioEventEmitter>();
    }


    /// <summary>
    /// BGM 파라미터 값을 바꿉니다.
    /// </summary>
    /// <param name="name">파라미터 이름</param>
    /// <param name="value">파라미터 값</param>
    //public void BGMChange(string name, float value)
    //{
    //    bgmInstance.getTimelinePosition(out bgmTime);
    //    bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //    bgmInstance.setTimelinePosition(bgmTime);
    //    bgmInstance.setParameterByName(name, 1.0f);
    //    bgmInstance.start();
    //}

    private void SceneManagerOnactiveSceneChanged(Scene arg0, Scene arg1)
    {
        LoadBank(arg1);
    }

    public bool CompareScene(Scene scene, string sceneName)
    {
        return scene.name.Equals(sceneName);
    }

    public void LoadBank(Scene scene)
    {
        Debug.Log(scene.name + "Bank Load");
        RuntimeManager.LoadBank(scene.name);
    }
    public void PlayBGM() => bgmEmitter.Play();

    public void PlaySFX(string eventpath)
    {
        sfxInstance = RuntimeManager.CreateInstance("event:/SFX/" + eventpath);
        sfxInstance.start();
    }

    public void ChangeBGM(EventReference bgm)
    {
        bgmEmitter.ChangeEvent(bgm);
    }

    public void ChangeBGM(string param, float value)
    {
        bgmEmitter.SetParameter(param, value);
    }

    public void SetPauseBGM(bool pause) => bgmEmitter.SetPause(pause);

    public void SetMasterVolume(float value) => masterBus.setVolume(value);

    public void SetBGMVolume(float value) => bgmBus.setVolume(value);

    public void SetSFXVolume(float value) => sfxBus.setVolume(value);

    public void Clear()
    {
        bgmEmitter.Stop();

        //bgmInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //bgmInstance.release();
    }
}
