using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataHandler
{
    public void LoadData(GameData gameData);
    public void SaveData(GameData gameData);

}
