using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugModCameraUiController : UI_Base
{
    [SerializeField]
    private Button rightBtn;
    [SerializeField]
    private Button leftBtn;
    [SerializeField]
    private Button upBtn;
    [SerializeField]
    private Button downBtn;

    private bool isDebugMod = false;


    public override void Init()
    {
    }

    public bool EnterDebugMode()
    {
        if (isDebugMod)
        {
            return false;
        }
        isDebugMod = true;
        gameObject.SetActive(true);
        return true;
    }

    public void ExitDebugMode()
    {
        isDebugMod = false;
        gameObject.SetActive(false);
    }

    public void ButtonPressdCheckUpdate(float hor, float ver)
    {
        //colors.pressedColor 사용
    }

    public void ButtonDisabledCheckUpdate(float hor, float ver)
    {
        //interactable=false 사용, clampX.x 어쩌고 값 사용~?
    }
}
