using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugModCameraUiController : UI_Base
{
    [SerializeField]
    private Image rightBtn;
    [SerializeField]
    private Image leftBtn;
    [SerializeField]
    private Image upBtn;
    [SerializeField]
    private Image downBtn;

    [SerializeField]
    private Sprite normal;
    [SerializeField]
    private Sprite Pressed;
    [SerializeField]
    private Sprite Disabled;

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


    public void ButtonStateCheckUpdate(Vector3 pos, Vector2 clampX, Vector2 clampY, float hor, float ver)
    {
        if (pos.x >= clampX.y)
        {
            rightBtn.sprite = Disabled;
        }
        else
        {
            rightBtn.sprite = normal;
            if (hor > 0)
            {
                rightBtn.sprite = Pressed;
            }
            else if (hor == 0)
            {
                rightBtn.sprite = normal;
            }
        }

        if (pos.x <= clampX.x)
        {
            leftBtn.sprite = Disabled;
        }
        else
        {
            leftBtn.sprite = normal;
            if (hor < 0)
            {
                leftBtn.sprite = Pressed;
            }
            else if (hor == 0)
            {
                leftBtn.sprite = normal;
            }
        }

        if (pos.y >= clampY.y)
        {
            upBtn.sprite = Disabled;
        }
        else
        {
            upBtn.sprite = normal;
            if (ver > 0)
            {
                upBtn.sprite = Pressed;
            }
            else if (ver == 0)
            {
                upBtn.sprite = normal;
            }
        }

        if (pos.y <= clampY.x)
        {
            downBtn.sprite = Disabled;
        }
        else
        {
            downBtn.sprite = normal;
            if (ver < 0)
            {
                downBtn.sprite = Pressed;
            }
            else if (ver == 0)
            {
                downBtn.sprite = normal;
            }
        }
    }
}
