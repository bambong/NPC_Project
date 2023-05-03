using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject settingPanel;

    private bool isOpen = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isOpen)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
            }
        }
    }

    private void OpenPanel()
    {
        settingPanel.SetActive(true);
        isOpen = true;
    }

    private void ClosePanel()
    {
        settingPanel.SetActive(false);
        isOpen = false;
    }
}
