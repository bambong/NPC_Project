using System.Collections.Generic;
using UnityEngine;

public enum KEY_TYPE
{
    EXIT_KEY,
    DEBUGMOD_KEY,
    INTERACTION_KEY,
    SKIP_KEY,
    RUN_KEY,
    UP_KEY,
    LEFT_KEY, 
    DOWN_KEY,
    RIGHT_KEY,
    KEY_COUNT
}

public static class KeySetting 
{
    public static Dictionary<KEY_TYPE, KeyCode> defaultKeys;
    public static Dictionary<KEY_TYPE, KeyCode> tempKeys;
    public static Dictionary<KEY_TYPE, KeyCode> currentKeys;

    static KeySetting()
    {
        defaultKeys = new Dictionary<KEY_TYPE, KeyCode>()
        {
            {KEY_TYPE.EXIT_KEY, KeyCode.Escape},
            {KEY_TYPE.DEBUGMOD_KEY, KeyCode.R},
            {KEY_TYPE.INTERACTION_KEY, KeyCode.F},
            {KEY_TYPE.SKIP_KEY, KeyCode.Mouse0},
            {KEY_TYPE.RUN_KEY, KeyCode.LeftShift},
            {KEY_TYPE.UP_KEY, KeyCode.W},
            {KEY_TYPE.LEFT_KEY, KeyCode.A},
            {KEY_TYPE.DOWN_KEY, KeyCode.S},
            {KEY_TYPE.RIGHT_KEY, KeyCode.D},
        };

        currentKeys = new Dictionary<KEY_TYPE, KeyCode>(defaultKeys); //default로 채우기
        tempKeys = new Dictionary<KEY_TYPE, KeyCode>(defaultKeys); //empty -> 오류떠서 default로 채우기
    }

    public static void ApplyTempKeys() //apply button
    {
        currentKeys = new Dictionary<KEY_TYPE, KeyCode>(tempKeys);
        tempKeys = new Dictionary<KEY_TYPE, KeyCode>(currentKeys);
    }


    public static void SetAllKeysToDefault() //init button
    {
        tempKeys = new Dictionary<KEY_TYPE, KeyCode>(defaultKeys);
        currentKeys = new Dictionary<KEY_TYPE, KeyCode>(defaultKeys);
    }

    public static void CopyCurrentToTemp() //메뉴 버튼에 할당
    {
        tempKeys = new Dictionary<KEY_TYPE, KeyCode>(currentKeys);
    }
    public static KeyCode GetKeyCode(KEY_TYPE type) => currentKeys[type];
}










public class KeyMappingController : MonoBehaviour
{
    [SerializeField]
    private GameObject InputBackgroundPanel;
    [SerializeField]
    private IntroStateWarningFrameController warning;

    private int key = -1;
    private bool isPanel = false;

    private void FixedUpdate()
    {
        if(isPanel && Input.anyKey)
        {
            OffInputBackgroundPanel();
            isPanel = false;
        }
    }

    public void CopyCurrentToTemp()
    {
        KeySetting.CopyCurrentToTemp();
    }

    public void SetAllKeysToDefault() //init
    {
        KeySetting.SetAllKeysToDefault();
    }

    #region
    public void StoreTempKeyMap()
    {
        KeySetting.tempKeys = new Dictionary<KEY_TYPE, KeyCode>(KeySetting.defaultKeys);
    }

    public void ApplyTempKeyMap()
    {
        Debug.Log("TempKeys contents:");
        foreach (var kvp in KeySetting.tempKeys)
        {
            Debug.Log(kvp.Key + ": " + kvp.Value);
        }
        CheckRepeatedKeys();
    }

    private void CheckRepeatedKeys()
    {
        HashSet<KeyCode> keySet = new HashSet<KeyCode>();
        bool hasDuplicated = false;
        int index = 0;

        foreach(var kvp in KeySetting.tempKeys)
        {
            if (index < (int)KEY_TYPE.KEY_COUNT)
            {
                if (keySet.Contains(kvp.Value))
                {
                    hasDuplicated = true;
                    warning.WarningCoroutine();
                    break;
                }
                else
                {
                    keySet.Add(kvp.Value);
                }
            }
            index++;
        }

        if(!hasDuplicated)
        {
            KeySetting.ApplyTempKeys();
        }
    }

    public void CancelTempKeyMap()
    {
        if (KeySetting.tempKeys != KeySetting.currentKeys) //저장 안했다
        {
            KeySetting.tempKeys = new Dictionary<KEY_TYPE, KeyCode>(KeySetting.currentKeys);
        }
    }
    #endregion


    private void OnGUI()
    {
        Event keyEvent = Event.current;

        if (keyEvent.isKey)
        {
            KeySetting.tempKeys[(KEY_TYPE)key] = keyEvent.keyCode;
            key = -1;
        }
    }

    public float GetHorizontal()
    {
        var horizontalValue = Input.GetKey(KeySetting.GetKeyCode(KEY_TYPE.RIGHT_KEY)) ? 1f : (Input.GetKey(KeySetting.GetKeyCode(KEY_TYPE.LEFT_KEY)) ? -1f : 0f);
        return horizontalValue;
    }

    public float GetVertical()
    {
        var verticalValue = Input.GetKey(KeySetting.GetKeyCode(KEY_TYPE.UP_KEY)) ? 1f : (Input.GetKey(KeySetting.GetKeyCode(KEY_TYPE.DOWN_KEY)) ? -1f : 0f);
        return verticalValue;
    }

    public KeyCode ReturnKey(KEY_TYPE keyType)
    {
        return KeySetting.currentKeys[keyType];
    }

    public void ChangeKey(int num)
    {
        //할당 하라는 문구 ON
        OnInputBackgroundPanel();
        isPanel = true;
        key = num;
    }

    private void OnInputBackgroundPanel()
    {
        InputBackgroundPanel.SetActive(true);
    }

    private void OffInputBackgroundPanel()
    {
        InputBackgroundPanel.SetActive(false);
    }
}

