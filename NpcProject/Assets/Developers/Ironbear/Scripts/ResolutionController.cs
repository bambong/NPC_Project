using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResolutionController : MonoBehaviour
{
    [SerializeField]
    bool is16v9;
    [SerializeField]
    bool hasHz;
    [SerializeField]
    Toggle fullscreenToggle;
    [SerializeField]
    TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private GameObject resolutionPanel;

    List<Resolution> resolutions = new List<Resolution>();

    private Animator animator;

    private bool isOut = false;
    private int resolutionNum;
    FullScreenMode screenMode;

    public int ResolutionIndex
    {
        get => PlayerPrefs.GetInt("ResolutionIndex", 0);
        set => PlayerPrefs.SetInt("ResolutionIndex", value);
    }

    public bool IsFullscreen
    {
        get => PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
        set => PlayerPrefs.SetInt("IsFullscreen", value ? 1 : 0);
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        InitUI();
    }

    void InitUI()
    {
        for(int i=0; i<Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        resolutionDropdown.options.Clear();

        int optionNum = 0;

        foreach(Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height;
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
                optionNum++;
            }
        }
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }

    //ui panel animation
    public void InAndOut()
    {
        if(!isOut)
        {
            animator.SetBool("isOut", true);
            isOut = true;
        }
        else if(isOut)
        {
            animator.SetBool("isOut", false);
            isOut = false;
        }
    }

    /*
    void SetResolution()
    {
        resolutions = new List<Resolution>(Screen.resolutions);
        resolutions.Reverse();

        

        if (is16v9)
        {
            resolutions = resolutions.FindAll(x => (float)x.width / x.height == 16f / 9);
        }

        if (!hasHz && resolutions.Count > 0)
        {
            List<Resolution> tempResolutions = new List<Resolution>();
            int curWidth = resolutions[0].width;
            int curHeight = resolutions[0].height;
            tempResolutions.Add(resolutions[0]);

            foreach (var resolution in resolutions)
            {
                if (curWidth != resolution.width || curHeight != resolution.height)
                {
                    tempResolutions.Add(resolution);
                    curWidth = resolution.width;
                    curHeight = resolution.height;
                }
            }
            resolutions = tempResolutions;
        }

        List<string> options = new List<string>();
        foreach (var resolution in resolutions)
        {
            string option = $"{resolution.width} x {resolution.height}";
            if (hasHz)
            {
                option += $" {resolution.refreshRate}Hz";
            }
            options.Add(option);
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = ResolutionIndex;
        fullscreenToggle.isOn = IsFullscreen;

        resolutionDropdown.RefreshShownValue();

        DropdownOptionChanged(ResolutionIndex);
    }

    public void DropdownOptionChanged(int resolutionIndex)
    {
        ResolutionIndex = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    public void FullscreenToggleChanged(bool isFull)
    {
        IsFullscreen = isFull;
        Screen.fullScreen = isFull;
    }
    */
}