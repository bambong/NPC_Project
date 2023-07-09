using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PausePanelController : BasePanelController
{
    [SerializeField]
    private SettingPanelController settingPanelController;

    public SettingPanelController SettingPanelController { get => settingPanelController;}

    public override void Init()
    {
        DontDestroyOnLoad(panel.gameObject);
        panel.gameObject.SetActive(false);
        settingPanelController.Init();
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
