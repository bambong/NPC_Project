using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum KEY_TYPE
{
    EXIT_KEY,
    DEBUGMOD_KEY,
    INTERACTION_KEY,
    TALK_KEY,
    SKIP_KEY,
    RETRY_KEY
}

public class KeyMappingController
{
    private Dictionary<KEY_TYPE, KeyCode> keyMap = new Dictionary<KEY_TYPE, KeyCode>()
    {
        {KEY_TYPE.EXIT_KEY, KeyCode.Escape},
        {KEY_TYPE.DEBUGMOD_KEY, KeyCode.G},
        {KEY_TYPE.INTERACTION_KEY, KeyCode.F},
        {KEY_TYPE.TALK_KEY, KeyCode.X},
        {KEY_TYPE.SKIP_KEY, KeyCode.X},
        {KEY_TYPE.RETRY_KEY, KeyCode.Return}
    };

    public KeyCode ReturnKey(KEY_TYPE keyType)
    {
        return keyMap[keyType];
    }

    public void ChangeKey(KEY_TYPE keyType, KeyCode keyCode)
    {
        keyMap[keyType] = keyCode;
    }
}
