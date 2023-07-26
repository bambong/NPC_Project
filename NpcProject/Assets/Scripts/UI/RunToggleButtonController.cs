using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunToggleButtonController : MonoBehaviour
{
    [SerializeField]
    private Image disableImage;
    [SerializeField]
    private Image enableImage;

    private bool curStatus;
    
    public void Open() 
    {
        curStatus = Managers.Data.CurrentSettingData.isToggleRun;
        ToggleImageUpdate();
    }
    public void OnToggleButtonActive() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        curStatus = !curStatus;
        Managers.Data.CurrentSettingData.isToggleRun = curStatus;
        ToggleImageUpdate();
    }
    public void ToggleImageUpdate() 
    {
        enableImage.gameObject.SetActive(curStatus);
        disableImage.gameObject.SetActive(!curStatus);
    }
}
