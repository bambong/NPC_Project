using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vSyncToggleButtonController : MonoBehaviour
{
    [SerializeField]
    private Image disableImage;
    [SerializeField]
    private Image enableImage;

    private bool curStatus;

    public void Open()
    {
        curStatus = Managers.Data.CurrentSettingData.isvSync;
        ToggleImageUpdate();
    }
    public void OnToggleButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        curStatus = !curStatus;
        Managers.Data.CurrentSettingData.isvSync = curStatus;
        ToggleImageUpdate();
        //Debug.Log(QualitySettings.vSyncCount);
    }

    public void ToggleImageUpdate()
    {
        enableImage.gameObject.SetActive(curStatus);
        disableImage.gameObject.SetActive(!curStatus);

        if(curStatus)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
}
