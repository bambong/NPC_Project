using System.Collections.Generic;
using UnityEngine;

public enum KEY_TYPE
{
    EXIT_KEY,
    DEBUGMOD_KEY,
    INTERACTION_KEY,
    TALK_KEY,
    SKIP_KEY,
    RUN_KEY,
    PAUSE_KEY,
    KEY_COUNT
}

public static class KeySetting { public static Dictionary<KEY_TYPE, KeyCode> keys = new Dictionary<KEY_TYPE, KeyCode>(); }

public class KeyMappingController : MonoBehaviour
{
    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.Escape, KeyCode.R, KeyCode.F, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.LeftShift, KeyCode.Escape };

    int key = -1;

    private void Awake()
    {
        for (int i = 0; i < (int)KEY_TYPE.KEY_COUNT; i++)
        {
            KeySetting.keys.Add((KEY_TYPE)i, defaultKeys[i]);
        }
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;

        if (keyEvent.isKey)
        {
            KeySetting.keys[(KEY_TYPE)key] = keyEvent.keyCode;
            key = -1;
        }
    }

    //public Dictionary<KEY_TYPE, KeyCode> keyMap = new Dictionary<KEY_TYPE, KeyCode>()
    //{
    //    {KEY_TYPE.EXIT_KEY, KeyCode.Escape},
    //    {KEY_TYPE.DEBUGMOD_KEY, KeyCode.R},
    //    {KEY_TYPE.INTERACTION_KEY, KeyCode.F},
    //    {KEY_TYPE.TALK_KEY, KeyCode.Mouse0},
    //    {KEY_TYPE.SKIP_KEY, KeyCode.Mouse1},
    //    {KEY_TYPE.RETRY_KEY, KeyCode.Return},
    //    {KEY_TYPE.RUN_KEY, KeyCode.LeftShift}
    //};

    public KeyCode ReturnKey(KEY_TYPE keyType)
    {
        return KeySetting.keys[keyType];
    }

    public void ChangeKey(int num)
    {
        key = num;
    }
}

