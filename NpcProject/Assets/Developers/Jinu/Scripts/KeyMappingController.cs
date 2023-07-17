using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public static Dictionary<KEY_TYPE, KeyCode> keys;
    
    static KeySetting()
    {
        keys = new Dictionary<KEY_TYPE, KeyCode>()
        {
            {KEY_TYPE.EXIT_KEY, KeyCode.Escape},
            {KEY_TYPE.DEBUGMOD_KEY, KeyCode.R},
            {KEY_TYPE.INTERACTION_KEY, KeyCode.F},
            {KEY_TYPE.SKIP_KEY, KeyCode.Mouse0},
            {KEY_TYPE.RUN_KEY, KeyCode.LeftShift},
            {KEY_TYPE.UP_KEY, KeyCode.W},
            {KEY_TYPE.LEFT_KEY, KeyCode.A},
            {KEY_TYPE.DOWN_KEY, KeyCode.S},
            {KEY_TYPE.RIGHT_KEY, KeyCode.D}
        };
    }

    public static void SetAllKeysToDefault()
    {
        var keyList = new List<KEY_TYPE>(keys.Keys);
        foreach (var key in keyList)
        {
            keys[key] = GetDefaultKeyCode(key);
        }
    }

    private static KeyCode GetDefaultKeyCode(KEY_TYPE keyType)
    {
        switch (keyType)
        {
            case KEY_TYPE.EXIT_KEY:
                return KeyCode.Escape;
            case KEY_TYPE.DEBUGMOD_KEY:
                return KeyCode.R;
            case KEY_TYPE.INTERACTION_KEY:
                return KeyCode.F;
            case KEY_TYPE.SKIP_KEY:
                return KeyCode.Mouse0;
            case KEY_TYPE.RUN_KEY:
                return KeyCode.LeftShift;
            case KEY_TYPE.UP_KEY:
                return KeyCode.W;
            case KEY_TYPE.LEFT_KEY:
                return KeyCode.A;
            case KEY_TYPE.DOWN_KEY:
                return KeyCode.S;
            case KEY_TYPE.RIGHT_KEY:
                return KeyCode.D;

            default:
                return KeyCode.None;
        }
    }
}

public class KeyMappingController : MonoBehaviour
{
    [SerializeField]
    private GameObject InputBackgroundPanel;

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

    //입력 키 초기화
    public void SetAllKeysToDefault()
    {
        KeySetting.SetAllKeysToDefault();
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

    public KeyCode ReturnKey(KEY_TYPE keyType)
    {
        return KeySetting.keys[keyType];
    }

    public void ChangeKey(int num)
    {
        //할당 하라는 문구 ON
        OnInputBackgroundPanel();
        isPanel = true;
        key = num;

        KEY_TYPE keyType = GetKeyTypeFromInt(num);
        if (keyType != KEY_TYPE.KEY_COUNT)
        {
            KeyCode newKeyCode = GetKeyCodeFromInt(num);
            KeySetting.keys[keyType] = newKeyCode;
            ModifyInputManagerAxes(keyType, newKeyCode);
        }
    }

    private void OnInputBackgroundPanel()
    {
        InputBackgroundPanel.SetActive(true);
    }

    private void OffInputBackgroundPanel()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    InputBackgroundPanel.SetActive(false);
        //}
        InputBackgroundPanel.SetActive(false);
    }


    private void ModifyInputManagerAxes(KEY_TYPE keyType, KeyCode newKeyCode)
    {
        Object[] inputManagerAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset");
        if (inputManagerAssets.Length > 0)
        {
            SerializedObject inputManagerObj = new SerializedObject(inputManagerAssets[0]);
            SerializedProperty axesArray = inputManagerObj.FindProperty("m_Axes");

            for (int i = 0; i < axesArray.arraySize; i++)
            {
                SerializedProperty axis = axesArray.GetArrayElementAtIndex(i);

                if (axis.FindPropertyRelative("m_Name").stringValue == GetAxisName(keyType))
                {
                    SerializedProperty altNegativeButton = axis.FindPropertyRelative("altNegativeButton");
                    SerializedProperty altPositiveButton = axis.FindPropertyRelative("altPositiveButton");

                    altNegativeButton.stringValue = newKeyCode.ToString();
                    altPositiveButton.stringValue = newKeyCode.ToString();
                    break;
                }
            }

            inputManagerObj.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.LogError("InputManager.asset not found");
        }
    }
    
    private KeyCode GetKeyCodeFromInt(int num)
    {
        switch(num)
        {
            case 0:
                return KeyCode.Escape;
            case 1:
                return KeyCode.R;
            case 2:
                return KeyCode.F;
            case 3:
                return KeyCode.Mouse0;
            case 4:
                return KeyCode.LeftShift;

            //axis
            case 5:
                return KeyCode.W;
            case 6:
                return KeyCode.A;
            case 7:
                return KeyCode.S;
            case 8:
                return KeyCode.D;

            default:
                return KeyCode.None;
        }
    }

    private KEY_TYPE GetKeyTypeFromInt(int num)
    {
        if (num >= 0 && num < (int)KEY_TYPE.KEY_COUNT)
        {
            return (KEY_TYPE)num;
        }
        else
        {
            return KEY_TYPE.KEY_COUNT;
        }
    }

    private string GetAxisName(KEY_TYPE keyType)
    {
        switch (keyType)
        {
            case KEY_TYPE.LEFT_KEY:
            case KEY_TYPE.RIGHT_KEY:
                return "Horizontal";
            case KEY_TYPE.UP_KEY:
            case KEY_TYPE.DOWN_KEY:
                return "Vertical";


            default:
                return string.Empty;
        }
    }
}

