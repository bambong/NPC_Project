using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestVolume : MonoBehaviour
{
    public Slider masterSlider;

    public Slider bgmSlider;

    public Slider sfxSlider;

    public void UpdateMasterVolume()
    {
        Managers.Sound.SetMasterVolume(masterSlider.value);
    }

    public void UpdateBGMVolume()
    {
        Managers.Sound.SetBGMVolume(bgmSlider.value);
    }

    public void UpdateSFXVolume()
    {
        Managers.Sound.SetSFXVolume(sfxSlider.value);
    }
}
