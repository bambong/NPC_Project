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

    private EventSystem eventSystem;
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;
    public EventSystem EventSystem { get => eventSystem; }
    void Awake()
    {
        Init();
      
    }
   
    protected virtual void Init()
    {
        eventSystem = FindObjectOfType<EventSystem>();     
        if (eventSystem == null) 
        {
            var go = Managers.Resource.Instantiate("UI/EventSystem");
            go.name = "@EventSystem";
            eventSystem = go.GetComponent<EventSystem>();
        }
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
