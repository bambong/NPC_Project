using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ResolutionSettingPanelController : ButtonBasePanelController
{
    [Header("Controllers")]
    [SerializeField]
    private StartPausePanelController startPausePanelController;

    [Header("Dropdowns")]
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private TMP_Dropdown screenmodeDropdown;
    [SerializeField]
    private TMP_Dropdown framerateDropdown;
    [SerializeField]
    private vSyncToggleButtonController vSyncToggle;

    [Header("ApplyTextUI")]
    [SerializeField]
    private GameObject textGo;
    [SerializeField]
    private TMP_Text applyText;
    [SerializeField]
    private Color applyColor;

    private List<Resolution> resolutions = new List<Resolution>();
    private List<FullScreenMode> screenmodes = new List<FullScreenMode>();
    private List<int> rates = new List<int>();
    private int resolutionNum;
    private int screenmodeNum;
    private int rateNum;
    private Sequence seq;
    private bool isPlay = false;

    private void Start()
    {
        textGo.transform.localScale = Vector3.zero;
        Init();
    }

    private void Awake()
    {
        isPlay = false;
        seq = null;

        //현재 적용된 값들 드롭다운에 표시
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

        if(isPlay)
        {
            return;
        }
        isPlay = true;
        applyText.color = applyColor;

        ApplyingCoroutine();
    }

    public void ApplyingCoroutine()
    {
        if (seq != null)
        {
            seq.Kill();
        }
        textGo.SetActive(true);
        textGo.transform.localScale = new Vector3(1f, 1f, 1f);
        Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
        Transform warningTransfom = textGo.gameObject.transform;
        seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(warningTransfom.DOScale(targetScale, 1f).SetEase(Ease.OutElastic));
        seq.Join(warningTransfom.DOShakePosition(1f));
        seq.AppendInterval(1.5f);
        seq.AppendCallback(
            () =>
            {
                isPlay = false;
                textGo.gameObject.SetActive(false);
                seq = null;
            }
        );
        seq.Play();
    }

    public void PlayButtonSound() => Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
}
