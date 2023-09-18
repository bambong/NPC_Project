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
    private RunToggleButtonController runToggle;

    [SerializeField]
    private Button defaultButton;

    private bool isPanel = false;
    private KeyMappingButtonController curKeyButton;

    public bool IsPanel { get => isPanel;  }

    protected override void OnOpen()
    {
        base.OnOpen();
        //CopyCurrentToTemp();
        runToggle.Open();
      // UpdateButtonText();
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
            if (Input.GetKeyDown(KeyCode.Space))
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

                KeySetting.Instance.tempKeys[curKeyButton.MyType] = curCode;
                curKeyButton.InputSpace();
                OffInputBackgroundPanel();
                isPanel = false;

                curKeyButton = null;
                yield break;
            }
            else if (Input.anyKeyDown)
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
 
                KeySetting.Instance.tempKeys[curKeyButton.MyType] = curCode;
                OffInputBackgroundPanel();
                isPanel = false;

                UpdateButtonText();
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
        KeySetting.Instance.CopyCurrentToTemp();
    }

    public void SetAllKeysToDefault() //init
    {
        KeySetting.Instance.SetAllKeysToDefault();
    }   

    public void OnApplyButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        Debug.Log("TempKeys contents:");
        foreach (var kvp in KeySetting.Instance.tempKeys)
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

        foreach (var kvp in KeySetting.Instance.tempKeys)
        {
            if (index < (int)KEY_TYPE.KEY_COUNT)
            {
                if (keySet.Contains(kvp.Value) || kvp.Value == KeyCode.Space)
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
            KeySetting.Instance.ApplyTempKeys();
        }
    }

    public void OnDefaultSettingButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        SetAllKeysToDefault();
        UpdateButtonText();
    }
    public void UpdateButtonText() 
    {
        for (int i = 0; i < keyMappingButtons.Count; ++i)
        {
            keyMappingButtons[i].UpdateText();
        }
    }


}
