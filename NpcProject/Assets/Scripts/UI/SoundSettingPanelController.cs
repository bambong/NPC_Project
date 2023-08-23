
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingPanelController : ButtonBasePanelController
{
    [SerializeField]
    private PausePanelController pausePanelController;

    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private TextMeshProUGUI masterText;

    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private TextMeshProUGUI bgmText;

    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private TextMeshProUGUI sfxText;

    [SerializeField]
    private Button defaultButton;

    protected override void OnOpen()
    {
        base.OnOpen();
        bgmSlider.value = Managers.Data.CurrentSettingData.bgmVolume;
        sfxSlider.value = Managers.Data.CurrentSettingData.sfxVolume;
        defaultButton.onClick.AddListener(OnDefaultSettingButtonActive);
    }

    protected override void OnClose()
    {
        base.OnClose();
        defaultButton.onClick.RemoveListener(OnDefaultSettingButtonActive);
    }

    public override void Init()
    {
        panel.gameObject.SetActive(false);
        masterSlider.onValueChanged.AddListener(OnChangeMasterVolume);
        bgmSlider.onValueChanged.AddListener(OnChangeBgmVolume);
        sfxSlider.onValueChanged.AddListener(OnChangeSfxVolume);
    }
  
    public void OnCloseButtonActive() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        pausePanelController.ChangeToMainMenuPanel();
    }

    public void OnChangeBgmVolume(float value) 
    {
        Managers.Data.SetBgmVolumeData(value);
        bgmText.text = ((int)(value * 100)).ToString();
    }
    public void OnChangeSfxVolume(float value) 
    { 
        Managers.Data.SetSfxVolumeData(value);
        sfxText.text = ((int)(value * 100)).ToString();
    }
    public void OnChangeMasterVolume(float value)
    {
        Managers.Data.SetMasterVolumeData(value);
        masterText.text = ((int)(value * 100)).ToString();
    }
    public void PlayButtonSound()=> Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);

    public void OnDefaultSettingButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        OnChangeMasterVolume(1);
        masterSlider.value = 1;
        OnChangeBgmVolume(1);
        bgmSlider.value = 1;
        OnChangeSfxVolume(1);
        sfxSlider.value = 1;
    }
}
