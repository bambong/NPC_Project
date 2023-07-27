using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSideButtonController : MonoBehaviour
{
    [SerializeField]
    private TutorialDescriptionPanelController descriptionPanelController;
    [SerializeField]
    private Button button;

    [SerializeField]
    private TextMeshProUGUI buttonText;
    [SerializeField]
    private Image highlightImage;

    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color highlightColor;

    private PauseTutorialPartData currentData;
    private int myIndex;
    public PauseTutorialPartData CurrentData { get => currentData;  }
    public int MyIndex { get => myIndex;  }

    private void Start()
    {
        button.onClick.AddListener(OnButtonActive);
    }
    public void SetInfo(int index, PauseTutorialPartData data) 
    {
        myIndex = index;
        buttonText.text = $"{index+1}. {data.title}";
        currentData = data;
    }
    public void OnHighlight(bool isOn) 
    {
        if (isOn) 
        {
            button.interactable = false;
            highlightImage.color = highlightColor;

        }
        else 
        {
            button.interactable = true;
            highlightImage.color = normalColor;
        }
    }

    public void OnButtonActive() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        descriptionPanelController.ChangePage(myIndex);
    }
}
