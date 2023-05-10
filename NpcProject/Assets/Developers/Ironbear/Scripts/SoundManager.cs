using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class SoundManager
{    
    private Bus masterBus;
    private Bus bgmBus;
    private Bus sfxBus;

    public EventInstance bgmInstance;
    public EventInstance sfxInstance;

    private int bgmTime;

    public void Init()
    {
        masterBus = RuntimeManager.GetBus("bus:/Master");
        bgmBus = RuntimeManager.GetBus("bus:/Master/BGM");
        sfxBus = RuntimeManager.GetBus("bus:/Master/SFX");

        //SceneManager.activeSceneChanged += SceneManagerOnactiveSceneChanged;
        //SceneManagerOnactiveSceneChanged(SceneManager.GetSceneByName("NULL"), SceneManager.GetActiveScene());
    }

    public void BGMPlay()
    {
        bgmInstance = RuntimeManager.CreateInstance("event:/BGM/OMG");
        bgmInstance.start();
    }    

    /// <summary>
    /// BGM 파라미터 값을 바꿉니다.
    /// </summary>
    /// <param name="name">파라미터 이름</param>
    /// <param name="value">파라미터 값</param>
    public void BGMChange(string name, float value)
    {
        bgmInstance.getTimelinePosition(out bgmTime);
        bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bgmInstance.setTimelinePosition(bgmTime);
        bgmInstance.setParameterByName(name, 1.0f);
        bgmInstance.start();
    }

    private void SceneManagerOnactiveSceneChanged(Scene arg0, Scene arg1)
    {
        LoadBank(arg1);

        if (CompareScene(arg1, "Unknown"))
        {
            ChangeBGM(Define.Scene.Unknown);
        }
        else if (CompareScene(arg1, "Clear"))
        {
            ChangeBGM(Define.Scene.Clear);
        }
        else if (CompareScene(arg1, "Chapter_01"))
        {
            ChangeBGM(Define.Scene.Chapter_01);
        }
        else if (CompareScene(arg1, "Chapter_01_Puzzle_01"))
        {
            ChangeBGM(Define.Scene.Chapter_01_Puzzle_01);
        }        
        else if (CompareScene(arg1, "Chapter_01_Puzzle_02"))
        {
            ChangeBGM(Define.Scene.Chapter_01_Puzzle_02);
        }        
        else if (CompareScene(arg1, "Chapter_01_Puzzle_03"))
        {
            ChangeBGM(Define.Scene.Chapter_01_Puzzle_02);
        }
        else if (CompareScene(arg1, "Serverroom"))
        {
            ChangeBGM(Define.Scene.Serverroom);
        }
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

    public void ChangeBGM(Define.Scene stage)
    {
        int index = (int)stage;
        switch(stage)
        {
            case Define.Scene.Unknown:
                bgmInstance = RuntimeManager.CreateInstance("event:/BGM/OMG");
                break;
            case Define.Scene.Clear:
                bgmInstance = RuntimeManager.CreateInstance("event:/BGM/OMG");
                break;
            case Define.Scene.Chapter_01:
                bgmInstance = RuntimeManager.CreateInstance("event:/BGM/OMG");
                break;
            case Define.Scene.Chapter_01_Puzzle_01:
                bgmInstance = RuntimeManager.CreateInstance("event:/BGM/OMG");
                break;
            case Define.Scene.Chapter_01_Puzzle_02:
                bgmInstance = RuntimeManager.CreateInstance("event:/BGM/OMG");
                break;
            case Define.Scene.Chapter_01_Puzzle_03:
                bgmInstance = RuntimeManager.CreateInstance("event:/BGM/OMG");
                break;
            case Define.Scene.Serverroom:
                bgmInstance = RuntimeManager.CreateInstance("event:/BGM/OMG");
                break;
        }
        bgmInstance.start();
    }


    public void PlaySFX(string eventpath)
    {
        sfxInstance = RuntimeManager.CreateInstance("event:/SFX/" + eventpath);
        sfxInstance.start();
    }

    //public void SetPauseBGM(bool pause) => BGMEmitter.SetPause(pause);

    public void SetMasterVolume(float value) => masterBus.setVolume(value);

    public void SetBGMVolume(float value) => bgmBus.setVolume(value);

    public void SetSFXVolume(float value) => sfxBus.setVolume(value);

    public void Clear()
    {
        bgmInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        bgmInstance.release();
    }
}
