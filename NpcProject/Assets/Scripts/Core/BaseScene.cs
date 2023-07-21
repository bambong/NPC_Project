using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class BaseScene : MonoBehaviour 
{
    [SerializeField]
    private EventReference bgm;
    [SerializeField]
    protected Vector3 playerSpawnSpot;

    private EventSystem eventSystem;
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;
    public EventSystem EventSystem { get => eventSystem; }
    public Vector3 PlayerSpawnSpot { get => playerSpawnSpot;  }

    void Awake()
    {
        Init();
        LoadGame();
    }
    private void LoadGame() 
    {
        Managers.Data.LoadGame(SceneManager.GetActiveScene().name);
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
            Managers.Sound.PlayBGM();
        }
        else
        {
            Managers.Sound.StopBGM();
        }
    }
    public abstract void Clear();

    public virtual void SaveData() { }
    public virtual void LoadData() { }
}
