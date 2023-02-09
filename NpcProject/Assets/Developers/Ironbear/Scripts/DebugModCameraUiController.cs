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

    [SerializeField]
    private Sprite normalState;

    [SerializeField]
    private Vector2 clampX;
    [SerializeField]
    private Vector2 clampY;

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
        if (hor > 0)
        {
            rightBtn.image.sprite = rightBtn.spriteState.pressedSprite;
        }
        if (hor < 0)
        {
            leftBtn.image.sprite = leftBtn.spriteState.pressedSprite;
        }
        if (ver > 0)
        {
            upBtn.image.sprite = upBtn.spriteState.pressedSprite;
        }
        if (ver < 0)
        {
            downBtn.image.sprite = downBtn.spriteState.pressedSprite;
        }

        if (hor == 0)
        {
            rightBtn.image.sprite = normalState;
            leftBtn.image.sprite = normalState;
        }
        if (ver == 0)
        {
            upBtn.image.sprite = normalState;
            downBtn.image.sprite = normalState;
        }
    }

    public void ButtonDisabledCheckUpdate(Vector3 pos)
    {
        if (pos.x > clampX.y)
        {
            rightBtn.interactable = false;
        }
        if (rightBtn.interactable == false && pos.x <= 20)
        {
            rightBtn.interactable = true;
        }

        if (pos.x < clampX.x)
        {
            leftBtn.interactable = false;
        }
        if (leftBtn.interactable == false && pos.x >= -14)
        {
            leftBtn.interactable = true;
        }

        if (pos.y > clampY.y)
        {
            upBtn.interactable = false;
        }
        if (upBtn.interactable == false && pos.y <= 55)
        {
            upBtn.interactable = true;
        }

        if (pos.y < clampY.x)
        {
            downBtn.interactable = false;
        }
        if (downBtn.interactable == false && pos.y >= 32.5)
        {
            downBtn.interactable = true;
        }

    }
}
