
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelController : UI_Base
{
    [SerializeField]
    private RectTransform panel;

    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;


    private bool isOpen;
    public bool IsOpen { get => isOpen;  }

    public override void Init()
    {
        panel.gameObject.SetActive(false);
        bgmSlider.onValueChanged.AddListener(OnChangeBgmVolume);
        sfxSlider.onValueChanged.AddListener(OnChangeSfxVolume);
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
        PlayButtonSound();
        panel.gameObject.SetActive(false);
    }

    public void OnCloseButtonActive() 
    {
        Close();
    }

    public void OnChangeBgmVolume(float value) 
    {
        Managers.Sound.SetBGMVolume(value);
    }
    public void OnChangeSfxVolume(float value) 
    { 
        Managers.Sound.SetSFXVolume(value);
    }
    public void PlayButtonSound()=> Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);

}
