using DG.Tweening.Plugins.Core.PathCore;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class DataManager
{
    private int progress = 0;
    private MiniGameLevelData dataPuzzleLevel;
    private GameData lastGameData;
    private PurposeData purposeData;
    private PurposePanelController purposePanel;

    private Dictionary<string, GameData> sceneGameData = new Dictionary<string, GameData>();
    private Dictionary<string, bool> clearEventData= new Dictionary<string, bool>();
    public int Progress { get => progress; }
    public GameData LastGameData { get => lastGameData;  }
    public MiniGameLevelData DataPuzzleLevel { get => dataPuzzleLevel; }
    public Dictionary<string, GameData> SceneGameData { get => sceneGameData;  }
    public Dictionary<string, bool> ClearEventData { get => clearEventData;  }
    public PurposeData PurposeData { get => purposeData; }

    public bool isAvaildProgress(int progress) => progress >= this.progress; 
    public bool UpdateProgress(int progress) 
    {
        if (!isAvaildProgress(progress)) 
        {
            return false;
        }

        this.progress = progress;   
        if(Managers.Game.Player != null) 
        {
            Managers.Game.Player.PurposePanel.UpdatePurpose();
        }

        return true;
    }
    public string GetCurrentPurpose()
    {
       if(purposeData.progressPurposes.Count <= progress) 
       {
            return string.Empty; 
       }   

       return purposeData.progressPurposes[progress];
    }
    public bool IsClearEvent(string id)
    {
        if (!ClearEventData.ContainsKey(id)) 
        {
            return false;
        }
        return ClearEventData[id];
    }
    public void ClearEvent(string id) 
    {
        ClearEventData.AddOrUpdateValue(id, true);
    }
    public void UpdateDataPuzzleLevel(MiniGameLevelData level) 
    {
        dataPuzzleLevel = level;
    }
    public void Init() 
    {
        lastGameData = new GameData();
        purposeData = Resources.Load<PurposeData>("Data/PurposeData");
        
    }
    public bool LoadGame(string sceneName)
    {
        GameData tempData;
        if (!SceneGameData.TryGetValue(sceneName, out tempData))
        {
            return false;
        }
        var dataHandlers = FindAllDataHandler();

        foreach(var data in dataHandlers) 
        {
            data.LoadData(tempData);
        }
        return true;
    }
    public void SaveGame(string sceneName)
    {
        GameData tempData;
        if(!SceneGameData.TryGetValue(sceneName,out tempData)) 
        {
            tempData = new GameData();
            SceneGameData.Add(sceneName, tempData);
        }
        tempData.sceneName = sceneName;
        var dataHandlers = FindAllDataHandler();
        foreach (var data in dataHandlers)
        {
            data.SaveData(tempData);
        }
        lastGameData = tempData;
    }
    public List<IDataHandler> FindAllDataHandler() 
    {
        IEnumerable<IDataHandler> dataHandlers = Object.FindObjectsOfType<MonoBehaviour>(true).OfType<IDataHandler>();
        return new List<IDataHandler>(dataHandlers);
    }
}
