using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class KeyMappingController : MonoBehaviour
{
    public KeyCode exitKey = KeyCode.Escape;
    public KeyCode debugmodKey = KeyCode.G;
    public KeyCode interactionKey = KeyCode.F;
    public KeyCode talkKey = KeyCode.X;
    public KeyCode skipKey = KeyCode.X;
    
    public void ChangeKey(KeyCode changeKey, KeyCode keyCode)
    {
        if(changeKey == interactionKey)
        {
            ChangeInteractionKey(keyCode);
        }
        else if (changeKey == debugmodKey)
        {
            ChangeDebugmodKey(keyCode);
        }
        else if (changeKey == talkKey)
        {
            ChangeTalkKey(keyCode);
        }
        else if (changeKey == skipKey)
        {
            ChangeSkipKey(keyCode);   
        }
        else if (changeKey == exitKey)
        {
            ChangeExitKey(keyCode);
        }
    }


    public void ChangeInteractionKey(KeyCode keyCode)
    {
        interactionKey = keyCode;
    }

    public void ChangeDebugmodKey(KeyCode keyCode)
    {
        debugmodKey = keyCode;
    }

    public void ChangeTalkKey(KeyCode keyCode)
    {
        talkKey = keyCode;
    }

    public void ChangeSkipKey(KeyCode keyCode)
    {
        skipKey = keyCode;
    }

    private void ChangeExitKey(KeyCode keyCode)
    {
        exitKey = keyCode;
    }
}
