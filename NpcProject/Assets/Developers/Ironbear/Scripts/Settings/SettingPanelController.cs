using UnityEngine;

public class SettingPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private GameObject inputs;
    [SerializeField]
    private GameObject sounds;
    [SerializeField]
    private GameObject resolutions;

    private bool isInputs = true;
    private bool isSounds = false;
    private bool isResolutions = false;

    public void OnSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void OffSettingPanel()
    {
        settingPanel.SetActive(false);
    }

    public void OnInputs()
    {
        if(isSounds || isResolutions)
        {
            sounds.SetActive(false);
            resolutions.SetActive(false);

            isSounds = false;
            isResolutions = false;
        }
        inputs.SetActive(true);
        isInputs = true;
    }

    public void OnSounds()
    {
        if(isInputs || isResolutions)
        {
            inputs.SetActive(false);
            resolutions.SetActive(false);

            isInputs = false;
            isResolutions = false;
        }
        sounds.SetActive(true);
        isSounds = true;
    }

    public void OnResolutions()
    {
        if(isInputs || isSounds)
        {
            inputs.SetActive(false);
            sounds.SetActive(false);

            isInputs = false;
            isSounds = false;
        }
        resolutions.SetActive(true);
        isResolutions = true;
    }
}
