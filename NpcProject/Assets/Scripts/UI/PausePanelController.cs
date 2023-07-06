using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PausePanelController : UI_Base
{
    [SerializeField]
    private RectTransform panel;

    [SerializeField]
    private Button exitButton;
    
    private bool isOpen;



    public override void Init()
    {
        DontDestroyOnLoad(panel.gameObject);
        panel.gameObject.SetActive(false);
       
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
        panel.gameObject.SetActive(false);
    }

    public void OnPanelExitButtonActive() 
    {
        Managers.Game.SetStateNormal();
        EventSystem.current.SetSelectedGameObject(null);
    }
}
