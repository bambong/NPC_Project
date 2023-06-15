using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButtonController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup rootGroup;

    [SerializeField]
    private CanvasGroup openGroup;
    [SerializeField]
    private CanvasGroup closeGroup;
    
    [SerializeField]
    private Image buttonImage;
    
    [SerializeField]
    private Color closeColor;
    
    [SerializeField]
    private Button myButton;

    private bool isOpen = false;


    public void Init(Action action)
    {
        isOpen = true;
        rootGroup.alpha = 1;
        openGroup.alpha = 1;
        closeGroup.alpha = 0;
        buttonImage.color = Color.white;
        myButton.onClick.RemoveAllListeners();
        myButton.onClick.AddListener(() => { action?.Invoke(); });
    }
    public void SetPlayMod(bool isOn)
    {
        if (isOn) 
        {
            rootGroup.interactable = false;
            rootGroup.DOFade(0, 0.3f);
        }
        else
        {
            rootGroup.interactable = true;
            rootGroup.DOFade(1, 0.3f);
        }
    }
    public void SetOpenMod(Action action, float time) 
    {
        if (isOpen) 
        {
            return;
        }
        isOpen = true;
        myButton.onClick.RemoveAllListeners();
        myButton.onClick.AddListener(()=> {
            Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
            action?.Invoke(); });
        var seq = DOTween.Sequence();
        seq.Append(buttonImage.DOColor(Color.white, time));
        seq.Join(openGroup.DOFade(1, time));
        seq.Join(closeGroup.DOFade(0, time));
        seq.OnComplete(() => { myButton.interactable = true; });
        seq.Play();
    }
    public void SetCloseMod(Action action , float time) 
    {
        if (!isOpen)
        {
            return;
        }
        isOpen = false;
        myButton.onClick.RemoveAllListeners();
        myButton.onClick.AddListener(() => {
            Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
            action?.Invoke(); });
        var seq = DOTween.Sequence();
        seq.Append(buttonImage.DOColor(closeColor, time));
        seq.Join(openGroup.DOFade(0, time));
        seq.Join(closeGroup.DOFade(1, time));
        seq.OnComplete(() => { myButton.interactable = true; });
        seq.Play();
    }
}
