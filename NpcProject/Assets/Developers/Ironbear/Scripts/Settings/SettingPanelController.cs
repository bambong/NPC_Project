using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject settingPanel;

    public void OnSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void OffSettingPanel()
    {
        settingPanel.SetActive(false);
    }
}
