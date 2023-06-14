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

    [SerializeField]
    private CanvasGroup backPanelGroup;
    [SerializeField]
    private RectTransform layoutRect;
    [SerializeField]
    private CanvasGroup layoutCanvasGroup;

    [SerializeField]
    private Vector3 layoutStartPos;

    [SerializeField]
    private Image backimageA;
    [SerializeField]
    private Image backimageB;

    public bool IsSelect { get => isSelect; }
    
    public TextMeshProUGUI choiceTextA;
    public TextMeshProUGUI choiceTextB;
    public TextMeshProUGUI choiceTextC;

    private bool isSelect = false;
    public override void Init()
    {

    }
    public void AnimClear() 
    {
        choiceA.interactable = false;
        choiceB.interactable = false;
        choiceC.interactable = false;
        backPanelGroup.alpha = 0;
        layoutCanvasGroup.alpha = 0;
        layoutRect.anchoredPosition = layoutStartPos;
    }
    public void AddButtonEvent()
    {
        Debug.Log("Add Button Event");
        choiceA.onClick.AddListener(ChoiceSelected);
        choiceB.onClick.AddListener(ChoiceSelected);
        choiceC.onClick.AddListener(ChoiceSelected);
    }

    public void Active(int activeCount)
    {
        AnimClear();
        Sequence seq = DOTween.Sequence();
        if (activeCount == 2)
        {
            backimageA.gameObject.SetActive(true);
            backimageB.gameObject.SetActive(true);
            seq.Append(backPanelGroup.DOFade(1, 1f));
            seq.Append(layoutCanvasGroup.DOFade(1, 0.5f));
            seq.Join(layoutRect.DOAnchorPos(Vector3.zero,0.5f));
            seq.OnComplete(() =>
            {
                choiceA.interactable = true;
                choiceB.interactable = true;
            });
            seq.Play();
        }
        else if(activeCount ==3)
        {
            choiceA.gameObject.SetActive(true);
            choiceB.gameObject.SetActive(true);
            choiceC.gameObject.SetActive(true);
            seq.Append(backPanelGroup.DOFade(1, 1f));
            seq.Append(layoutCanvasGroup.DOFade(1, 0.5f));
            seq.Join(layoutRect.DOAnchorPos(Vector3.zero, 0.5f));
            seq.OnComplete(() =>
            {
                choiceA.interactable = true;
                choiceB.interactable = true;
                choiceC.interactable = true;
            });
            seq.Play();
        }
        else
        {
            isSelect = true;
            Debug.Log("The choice are not completely created");
        }
    }

    public void Inactive()
    {
        AnimClear();
        backimageA.gameObject.SetActive(false);
        backimageB.gameObject.SetActive(false);
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
