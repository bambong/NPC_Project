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

    public void BGMControl(Define.BGM bgmAction, float fadeDelay = 1.0f)
    {
        switch(bgmAction)
        {
            case Define.BGM.Start:
                Managers.Scene.CurrentScene.StartCoroutine(Start(fadeDelay));
                break;
            case Define.BGM.ReStart:
                Managers.Scene.CurrentScene.StartCoroutine(Restart(fadeDelay));
                break;
            case Define.BGM.Stop:
                Managers.Scene.CurrentScene.StartCoroutine(Stop(fadeDelay));
                break;
            case Define.BGM.Pause:
                Managers.Scene.CurrentScene.StartCoroutine(Pause(fadeDelay));
                break;
        }        
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
    private IEnumerator Start(float fadeDelay)
    {
        bgmEmitter.Play();

        for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime / fadeDelay)
        {
            float volume = Mathf.Lerp(1.0f, 0.0f, t);
            bgmEmitter.EventInstance.setParameterByName("Volume", volume);
            yield return null;
        }
    }
    private IEnumerator Restart(float fadeDelay)
    {
        for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime / fadeDelay)
        {
            float volume = Mathf.Lerp(1.0f, 0.0f, t);
            bgmEmitter.EventInstance.setParameterByName("Volume", volume);
            yield return null;
        }        
    }
    private IEnumerator Pause(float fadeDelay)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeDelay)
        {
            float volume = Mathf.Lerp(1.0f, 0.0f, t);
            bgmEmitter.EventInstance.setParameterByName("Volume", volume);
            yield return null;
        }
    }
    private IEnumerator Stop(float fadeDelay)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeDelay)
        {
            float volume = Mathf.Lerp(1.0f, 0.0f, t);
            bgmEmitter.EventInstance.setParameterByName("Volume", volume);
            yield return null;
        }
        bgmEmitter.Stop();
    }
    #endregion


    public void PlaySFX(Enum eventpath)
    {
        sfxInstance = RuntimeManager.CreateInstance("event:/SFX/" + eventpath.ToString());
        sfxInstance.start();
    }

    #region SoundVolumeControl
    public void SetPauseBGM(bool pause) => bgmEmitter.SetPause(pause);

    public void SetMasterVolume(float value) => masterBus.setVolume(value);

    public void SetBGMVolume(float value) => bgmBus.setVolume(value);

    public void SetSFXVolume(float value) => sfxBus.setVolume(value);
    #endregion

    public void Clear()
    {
    }

}
