using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugModCameraUiController : UI_Base
{
    private bool isDebugMod = false;

    public override void Init()
    {
    }

    public bool EnterDebugMode()
    {
        Managers.Game.SetStateDebugMod();
        if (isDebugMod)
        {
            return false;
        }
        isDebugMod = true;
        Debug.Log("create UI");
        CreateCameraUi();
        return true;
    }

    private void CreateCameraUi()
    {
        
        var arrowPrefab = Managers.Resource.Instantiate($"UI/ArrowsCanvas");
        arrowPrefab.name = "@Arrows";
    }

    public void ExitDebugMode()
    {
        isDebugMod = false;
    }

    private IEnumerator MoveCheckUpdate()
    {
        while (isDebugMod)
        {
            //이동 제한 및 ui 없애고 보여주는 기능
        }
        yield return null;
    }
}
