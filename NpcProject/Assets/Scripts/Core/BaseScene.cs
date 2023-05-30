using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour 
{
    [SerializeField]
    private EventReference bgm;
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        Init();
      
    }
   
    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }
    public virtual void PlayBgm() 
    {
        if (!bgm.IsNull)
        {
            Managers.Sound.ChangeBGM(bgm);
            Managers.Sound.BGMControl(Define.BGM.Start);
        }
        else
        {
            Managers.Sound.BGMControl(Define.BGM.Stop);
        }
    }
    public abstract void Clear();

    public virtual void SaveData() { }
    public virtual void LoadData() { }
}
