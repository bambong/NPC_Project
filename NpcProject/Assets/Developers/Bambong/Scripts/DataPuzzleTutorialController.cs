using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;


public class DataPuzzleTutorialController : BaseTutorialController
{
    [SerializeField]
    private HelpButtonController helpButton;
    [SerializeField]
    private CanvasGroup gameLayout;


    protected override void Start()
    {
         base.Start();
        if (!Managers.Data.IsClearEvent(GuId))
        {
            Open();
            onComplete.AddListener(() => Managers.Data.ClearEvent(GuId));
        }
        else
        {
            Close();
        }

    }

    public override bool Open()
    {
        if (isOpen)
        {
            return false;
        }
        isOpen = true;
        gameObject.SetActive(true);

        OnOpen();

        rootGroup.interactable = true;
        clickNotice.anchoredPosition = startPos;

        var seq = DOTween.Sequence();
        seq.AppendInterval(startDelay);
        seq.AppendCallback(() =>
        {
            onStart?.Invoke();
        });
        seq.Append(rootGroup.DOFade(1, openAnimTime));
        seq.Join(clickNotice.DOAnchorPos(desirePos, openAnimTime));
        seq.Join(gameLayout.DOFade(0, openAnimTime));
        seq.OnComplete(SetTutorial);
        seq.Play();
        return true;
    }
    public override bool Close()
    {
        if (isPlaying || !isOpen)
        {
            return false;
        }
        Managers.Sound.PlaySFX(Define.SOUND.AssignmentKeyword);
        isPlaying = true;
        prevButton.interactable = false;
        nextButton.interactable = false;
        var seq = DOTween.Sequence();
        seq.Append(rootGroup.DOFade(0, openAnimTime));
        seq.Join(clickNotice.DOAnchorPos(startPos, openAnimTime));
        seq.Join(gameLayout.DOFade(1, openAnimTime));
        seq.OnComplete(() =>
        {
            OnClose();
            rootGroup.interactable = false;
            isOpen = false;
            onComplete?.Invoke();
            clickNotice.anchoredPosition = startPos;
            rootGroup.alpha = 0;
            innerGroup.alpha = 0;
            renderImage.color = new Color(1, 1, 1, 0);
            descriptionImage.color = new Color(1, 1, 1, 0);
            pageMarks[curIndex].color = Color.white;
            curIndex = 0;
            gameObject.SetActive(false);
        });
        seq.Play();
        return true;
    }

    protected override void OnOpen()
    {
        helpButton.SetCloseMod(()=> { Close(); }, changeAnimTime);
    }
    protected override void OnClose()
    {
        helpButton.SetOpenMod(() => { Open(); }, openAnimTime);
    }



}
