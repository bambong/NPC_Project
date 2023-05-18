using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "MiniGameLevelData", menuName = "Scriptable Data/MiniGameLevelData", order = 2)]
public class MiniGameLevelData : ScriptableObject
{
    public int progress = 0;
    public string guId;

    public int row = 5;
    public int column = 5;
    public List<MiniGameManager.ColorOrder> colorOrders;
    public int[] answerColorKey;
    public string[] answerKey;

    [ContextMenu("Generate GUID")]
    private void GenerateGuid()
    {
        guId = System.Guid.NewGuid().ToString();
    }

}