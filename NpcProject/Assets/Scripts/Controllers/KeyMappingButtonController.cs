using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyMappingButtonController : MonoBehaviour
{
    [SerializeField]
    private KEY_TYPE myType;

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI innerText;

    [SerializeField]
    private InputSettingPanelController inputSetting;

    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color changedColor;
    [SerializeField]
    private Color repeatColor;
    public KEY_TYPE MyType { get => myType; }

    public void SetSelected() 
    {
        image.color = selectedColor;
    }

    public void UpdateText() 
    {
        if (KeySetting.Instance.tempKeys[myType] != KeySetting.Instance.defaultKeys[myType])
        {
            image.color = changedColor;
        }
        else
        {
            image.color = normalColor;
        }

        foreach (var item  in KeySetting.Instance.tempKeys) 
        {
            if(item.Key == myType) 
            {
                continue;
            }
            if (KeySetting.Instance.tempKeys[myType] == item.Value) 
            {
                image.color = repeatColor;
                break;
            }
        }
        innerText.text = KeySetting.Instance.tempKeys[myType].ToString();
    }

    public void OnButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        inputSetting.ChangeKey(this);
    }


}
