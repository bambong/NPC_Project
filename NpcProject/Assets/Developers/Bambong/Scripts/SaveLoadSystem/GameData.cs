using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class GameData
{
   public string sceneName;
   public Vector3 playerPos;
   public Dictionary<string, DebugZoneData> debugZoneDatas = new Dictionary<string, DebugZoneData>();
   public Dictionary<string,Dictionary<string, KeywordEntityData>> keywordEntityDatas = new Dictionary<string, Dictionary<string, KeywordEntityData>>();
}
public class GameSettingData
{
    public float masterVolume = 1;
    public float bgmVolume = 1;
    public float sfxVolume = 1;
    public bool isToggleRun = false;
    public bool isvSync = false;
}
public class KeywordEntityData 
{
    public class KeywordFrameData 
    {
        public bool isLock;
        public string id;
    }
    public bool isEnable;
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
    public List<KeywordFrameData> keywordFrameDatas = new List<KeywordFrameData>();
}
public class DebugZoneData
{
    public List<string> playerFramDatas = new List<string>();
   
}
