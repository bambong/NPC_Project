using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSettingPanelController : ButtonBasePanelController
{
    [SerializeField]
    private StartPausePanelController startPausePanelController;

    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private TMP_Dropdown screenmodeDropdown;
    [SerializeField]
    private TMP_Dropdown framerateDropdown;
    [SerializeField]
    private vSyncToggleButtonController vSyncToggle;

    private List<Resolution> resolutions = new List<Resolution>();
    private List<FullScreenMode> screenmodes = new List<FullScreenMode>();
    private List<int> rates = new List<int>();
    private int resolutionNum;
    private int screenmodeNum;
    private int rateNum;

    private void Start()
    {     
        Init();
    }

    public override void Init()
    {
        //panel.gameObject.SetActive(false);
        resolutions.AddRange(Screen.resolutions);
        screenmodes.Add(FullScreenMode.Windowed);
        screenmodes.Add(FullScreenMode.FullScreenWindow);
        screenmodes.Add(FullScreenMode.MaximizedWindow);
        rates.Add(30);
        rates.Add(60);
        rates.Add(120);
        rates.Add(144);

        resolutionDropdown.ClearOptions();
        screenmodeDropdown.ClearOptions();
        framerateDropdown.ClearOptions();

        //resolution
        int optionNum = 0;
        foreach(Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height;
            resolutionDropdown.options.Add(option);

            if(item.width==Screen.width && item.height==Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();

        //screenmode
        foreach (FullScreenMode mode in screenmodes)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = mode.ToString();
            screenmodeDropdown.options.Add(option);

            if (mode==Screen.fullScreenMode)
            {
                screenmodeDropdown.value = optionNum;
            }
            optionNum++;
        }
        screenmodeDropdown.RefreshShownValue();

        foreach (int rate in rates)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = rate.ToString();
            framerateDropdown.options.Add(option);

            if (rate == Application.targetFrameRate)
            {
                framerateDropdown.value = optionNum;
            }
            optionNum++;
        }
        framerateDropdown.RefreshShownValue();

        //int curFramerate = PlayerPrefs.GetInt("FrameRate", 60);
        //SetFrameRate(curFramerate);
    }

    private void SetFrameRate(int framerate)
    {
        Application.targetFrameRate = framerate;
        PlayerPrefs.SetInt("FrameRate", framerate);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        vSyncToggle.Open();
    }

    public void DropdownOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void DropdownScreenmodeChange(int x)
    {
        screenmodeNum = x;
    }

    public void DropdownRateChange(int x)
    {
        rateNum = x;
    }

    public void OnApplyButtonActive()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenmodes[screenmodeNum]);
        SetFrameRate(rates[rateNum]);
    }

    public void PlayButtonSound() => Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
}
