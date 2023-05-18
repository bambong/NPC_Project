using UnityEngine;
using System.IO;

public class NameInputScene : BaseScene
{
    [SerializeField]
    private GameObject inputfieldPanel;
    [SerializeField]
    private Vector3 playerSpawnSpot;

    private string logFilePath;

    public override void Clear()
    {

    }

    protected override void Init()
    {
        base.Init();

        var player = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        player.transform.position = playerSpawnSpot;
    }

    private void Awake()
    {
        Debug.unityLogger.logEnabled = true;
        logFilePath = Application.persistentDataPath + "/log.txt";
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        using (StreamWriter writer = File.AppendText(logFilePath))
        {
            writer.WriteLine(logString);
            if (type == LogType.Error || type == LogType.Exception)
            {
                writer.WriteLine(stackTrace);
            }
        }
    }

    private void OnDisable()
    {
        string logFileContents = File.ReadAllText(logFilePath);
        PlayerPrefs.SetString("debug.log", logFileContents);
    }
}

