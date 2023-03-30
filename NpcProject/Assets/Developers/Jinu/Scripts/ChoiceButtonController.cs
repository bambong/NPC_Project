using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ChoiceButtonController : UI_Base
{
    [SerializeField]
    private Button choiceA;
    [SerializeField]
    private Button choiceB;
    [SerializeField]
    private Button choiceC;

    public bool IsSelect { get => isSelect; }
    
    public TextMeshProUGUI choiceTextA;
    public TextMeshProUGUI choiceTextB;
    public TextMeshProUGUI choiceTextC;

    private bool isSelect = false;
    public override void Init()
    {

    }

    public void AddButtonEvent()
    {
        choiceA.onClick.AddListener(ChoiceSelected);
        choiceB.onClick.AddListener(ChoiceSelected);
        choiceC.onClick.AddListener(ChoiceSelected);
    }

    public void Active(int activeCount)
    {
        if(activeCount == 2)
        {
            choiceA.gameObject.SetActive(true);
            choiceB.gameObject.SetActive(true);
        }
        if(activeCount ==3)
        {
            choiceA.gameObject.SetActive(true);
            choiceB.gameObject.SetActive(true);
            choiceC.gameObject.SetActive(true);
        }
    }

    public void Inactive()
    {
        choiceA.gameObject.SetActive(false);
        choiceB.gameObject.SetActive(false);
        choiceC.gameObject.SetActive(false);
    }

    public void ChoiceSelected()
    {
        isSelect = true;
        Inactive();
    }

    public void SetisSelect(bool value)
    {
        isSelect = value;
    }
}
