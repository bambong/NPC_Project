
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelController : BasePanelController
{
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;

    public override void Init()
    {
        panel.gameObject.SetActive(false);
        masterSlider.onValueChanged.AddListener(OnChangeMasterVolume);
        bgmSlider.onValueChanged.AddListener(OnChangeBgmVolume);
        sfxSlider.onValueChanged.AddListener(OnChangeSfxVolume);
    }
  
    protected override void OnOpen() 
    {
        base.OnOpen();
        bgmSlider.value = Managers.Data.CurrentSettingData.bgmVolume;
        sfxSlider.value = Managers.Data.CurrentSettingData.sfxVolume;
    }
    protected override void OnClose() 
    {
        base.OnClose();
        PlayButtonSound();
    }

    public void OnCloseButtonActive() 
    {
        Close();
    }

    public void OnChangeBgmVolume(float value) 
    {
        Managers.Data.SetBgmVolumeData(value);
    }
    public void OnChangeSfxVolume(float value) 
    { 
        Managers.Data.SetSfxVolumeData(value);
    }
    public void OnChangeMasterVolume(float value)
    {
        Managers.Data.SetMasterVolumeData(value);
    }
    public void PlayButtonSound()=> Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);

}
