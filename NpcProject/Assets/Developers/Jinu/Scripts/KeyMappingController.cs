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
    RETRY_KEY,
    RUN_KEY
}

public class KeyMappingController
{
    private Dictionary<KEY_TYPE, KeyCode> keyMap = new Dictionary<KEY_TYPE, KeyCode>()
    {
        {KEY_TYPE.EXIT_KEY, KeyCode.Escape},
        {KEY_TYPE.DEBUGMOD_KEY, KeyCode.R},
        {KEY_TYPE.INTERACTION_KEY, KeyCode.F},
        {KEY_TYPE.TALK_KEY, KeyCode.Mouse0},
        {KEY_TYPE.SKIP_KEY, KeyCode.Mouse1},
        {KEY_TYPE.RETRY_KEY, KeyCode.Return},
        {KEY_TYPE.RUN_KEY, KeyCode.LeftShift}
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
