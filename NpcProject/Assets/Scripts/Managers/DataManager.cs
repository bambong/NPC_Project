using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData
{
    public SaveData(int progress , string sceneName , Vector3 playerPos) 
    {
        this.progress = progress;
        this.sceneName = sceneName;
        this.playerPos = playerPos;
    }
    public int progress;
    public string sceneName;
    public Vector3 playerPos;
}

public class DataManager
{
    private int progress = 0;
    private MiniGameLevelData dataPuzzleLevel;
    private SaveData lastSaveData;
    public int Progress { get => progress; }
    public MiniGameLevelData DataPuzzleLevel { get => dataPuzzleLevel; }
    public SaveData LastSaveData { get => lastSaveData;}

    // 진행도 타입, 진행 정도
    //ublic Dictionary<ProgressType, int> progressDict = new Dictionary<ProgressType, int>();
    // event ID , isClear
    private Dictionary<int, bool> eventDict = new Dictionary<int, bool>();
    public bool isAvaildProgress(int progress) => progress >= this.progress; 
    public bool UpdateProgress(int progress) 
    {
        if (!isAvaildProgress(progress)) 
        {
            return false;
        }
        this.progress = progress;   
        return true;
    }
    public bool AddOnceEvent(int id)
    {
        if (eventDict.ContainsKey(id)) 
        {
            return false;
        }
        eventDict.Add(id, false);
        return true;
    }
    public bool ClearOnceEvent(int id) 
    {
        if (!eventDict.ContainsKey(id))
        {
            Debug.LogError($"등록되지않은 이벤트 클리어요청 {id}");
            return false;
        }
        eventDict[id] = true;
        return true; 
    }
    public bool isClearEvent(int id)
    {
        if (!eventDict.ContainsKey(id))
        {    
            return false;
        }
        return eventDict[id];
    }
    public void UpdateDataPuzzleLevel(MiniGameLevelData level) 
    {
        dataPuzzleLevel = level;
    }
    public void SaveData(SaveData save) 
    {
        if (!isAvaildProgress(save.progress)) 
        {
            return;
        }

        lastSaveData = save;
    }
    public void Init() 
    {
    
    }
}
