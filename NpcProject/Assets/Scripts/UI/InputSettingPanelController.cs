using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputSettingPanelController : ButtonBasePanelController
{
    [SerializeField]
    private GameObject InputBackgroundPanel;
    [SerializeField]
    private KeyMappingStatusTextController keyMappingStatus;
    [SerializeField]
    private List<KeyMappingButtonController> keyMappingButtons;

    [SerializeField]
    private Button defaultButton;

    private bool isPanel = false;
    private KeyMappingButtonController curKeyButton;

    public bool IsPanel { get => isPanel;  }

    protected override void OnOpen()
    {
        CopyCurrentToTemp();
        base.OnOpen();
        for (int i = 0; i < keyMappingButtons.Count; ++i)
        {
            keyMappingButtons[i].UpdateText();
        }
        defaultButton.onClick.AddListener(OnDefaultSettingButtonActive);
    }

    protected override void OnClose()
    {
        base.OnClose();
        CopyCurrentToTemp();
        defaultButton.onClick.RemoveListener(OnDefaultSettingButtonActive);
    }
    IEnumerator CheckInputKey() 
    {
        while (isPanel) 
        {
           
            if (Input.anyKeyDown)
            {
                KeyCode curCode = KeyCode.None;
                
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        curCode = keyCode;
                        break; 
                    }
                }
 
                KeySetting.tempKeys[curKeyButton.MyType] = curCode;
                OffInputBackgroundPanel();
                isPanel = false;
                curKeyButton.UpdateText();
                curKeyButton = null;
                yield break;
            }
            
            yield return null;
        }
    }

    public void ChangeKey(KeyMappingButtonController button)
    {
        if (isPanel) 
        {
            return;
        }
        isPanel = true;
        button.SetSelected();
        OnInputBackgroundPanel();
        curKeyButton = button;
        StartCoroutine(CheckInputKey());
    }

    private void OnInputBackgroundPanel()
    {
        InputBackgroundPanel.SetActive(true);
    }

    private void OffInputBackgroundPanel()
    {
        InputBackgroundPanel.SetActive(false);
    }
    public void CopyCurrentToTemp()
    {
        KeySetting.CopyCurrentToTemp();
    }

    public void SetAllKeysToDefault() //init
    {
        KeySetting.SetAllKeysToDefault();
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

        foreach (var kvp in KeySetting.tempKeys)
        {
            if (index < (int)KEY_TYPE.KEY_COUNT)
            {
                if (keySet.Contains(kvp.Value))
                {
                    hasDuplicated = true;
                    keyMappingStatus.SetMappingSuccess(false);
                    break;
                }
                else
                {
                    keySet.Add(kvp.Value);
                }
            }
            index++;
        }

        if (!hasDuplicated)
        {
            keyMappingStatus.SetMappingSuccess(true);
            KeySetting.ApplyTempKeys();
        }
    }

    public void OnDefaultSettingButtonActive()
    {
        SetAllKeysToDefault();
        for(int i = 0; i < keyMappingButtons.Count; ++i)
        {
            keyMappingButtons[i].UpdateText();
        }

    }

}
