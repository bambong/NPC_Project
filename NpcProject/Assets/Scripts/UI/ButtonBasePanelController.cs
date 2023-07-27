using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonBasePanelController : BasePanelController
{
    [SerializeField]
    protected TextMeshProUGUI text;
    [SerializeField]
    protected Color disableColor;
    [SerializeField]
    protected Color selectedColor;

    public void SetSelected(bool isOn) 
    {
        if (isOn) 
        {
            text.color = selectedColor;
        }
        else 
        {
            text.color = disableColor;
        }
    
    }


}
