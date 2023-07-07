using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PausePanelController : UI_Base
{
    [SerializeField]
    private RectTransform panel;

    [SerializeField]
    private SettingPanelController settingPanelController;

    private bool isOpen;
    public SettingPanelController SettingPanelController { get => settingPanelController;}


    public override void Init()
    {
        DontDestroyOnLoad(panel.gameObject);
        panel.gameObject.SetActive(false);
        settingPanelController.Init();
    }

    public void Open()
    {
        if (isOpen)
        {
            return;
        }
        isOpen = true;
        panel.gameObject.SetActive(true);
    }
    public void Close() 
    {
        if (!isOpen)
        {
            return;
        }
        isOpen = false;
        panel.gameObject.SetActive(false);
    }
    public void OnSettingButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        settingPanelController.Open();
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnResumeButtonActive() 
    {
        Managers.Game.SetStateNormal();
        EventSystem.current.SetSelectedGameObject(null);
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
    }
    public void OnQuitButtonActive() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        Application.Quit();
    }


}
