using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private GameObject inputs;
    [SerializeField]
    private GameObject sounds;

    private bool isInputs = false;
    private bool isSounds = false;


    private void Awake()
    {
        isInputs = true;
    }

    public void OnSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void OffSettingPanel()
    {
        settingPanel.SetActive(false);
    }

    public void OnInputs()
    {
        if(isSounds)
        {
            sounds.SetActive(false);
            isSounds = false;
        }
        inputs.SetActive(true);
        isInputs = true;
    }

    public void OnSounds()
    {
        if(isInputs)
        {
            inputs.SetActive(false);
            isInputs = false;
        }
        sounds.SetActive(true);
        isSounds = true;
    }
}
