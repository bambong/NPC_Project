using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

[Serializable]
public class ServerroomTutorialData 
{
    public Sprite descriptionImage;
    public VideoClip videoClip;
    public Sprite keywordImage;
    [TextArea(2,10)]
    public string descriptionText;
}

public class ServerroomTutorialController : GuIdBehaviour
{
    [SerializeField]
    private bool isEventOnce;

    [SerializeField]
    private float startDelay;

    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private CanvasGroup rootGroup;

    [SerializeField]
    private CanvasGroup innerGroup;

    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button prevButton;
    [SerializeField]
    private Image keywordImage;
    [SerializeField]
    private RawImage renderImage;
    [SerializeField]
    private Image descriptionImage;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private List<ServerroomTutorialData> tutorialData;

    [SerializeField]
    private List<Image> pageMarks;
  
    [SerializeField]
    private Color pageMarkColor;

    [SerializeField]
    private float openAnimTime = 0.5f;

    [SerializeField]
    private float changeAnimTime = 0.3f;

    [SerializeField]
    private UnityEvent onStart;
    [SerializeField]
    private UnityEvent onComplete;

    [SerializeField]
    private RectTransform clickNotice;

    [SerializeField]
    private Vector3 startPos;
    [SerializeField]
    private Vector3 desirePos;

    private int curIndex;
    private bool isPlaying;
    private const float VIDEO_VISIBLE_TIME = 0.2f;
    protected override void Start()
    {
        for ( int i = tutorialData.Count; i < pageMarks.Count; ++i) 
        {
            pageMarks[i].gameObject.SetActive(false);
        }
        clickNotice.anchoredPosition = startPos;
        rootGroup.alpha = 0;
        innerGroup.alpha = 0;
        renderImage.color = new Color(1, 1, 1, 0);
        descriptionImage.color = new Color(1, 1, 1, 0);

        Open();
    }

    public void Open()
    {
        if (isEventOnce && Managers.Data.IsClearEvent(guId)) 
        {
            return;
        }
        rootGroup.interactable = true;
        clickNotice.anchoredPosition = startPos;
        Managers.Game.RetryPanel.CloseResetButton();
        Managers.Game.SetStateEvent();
        //Managers.Game.Player.SetstateStop();
        var seq = DOTween.Sequence();
        seq.AppendInterval(startDelay);
        seq.AppendCallback(() =>
        {
            onStart?.Invoke();
        });
        seq.Append(rootGroup.DOFade(1, openAnimTime));
        seq.Join(clickNotice.DOAnchorPos(desirePos, openAnimTime));
        seq.OnComplete(SetTutorial);
        seq.Play();
    }

    private void SetTutorial() 
    {
        isPlaying = true;
        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();
        if (curIndex > 0) 
        {
            prevButton.onClick.AddListener(() => UpdatePage(-1));
        }
        if (curIndex >= tutorialData.Count - 1) 
        {
            nextButton.onClick.AddListener(Close);
        }
        else 
        {
            nextButton.onClick.AddListener(()=>UpdatePage(+1));
        }
        var seq = DOTween.Sequence();
        seq.Append(innerGroup.DOFade(0, changeAnimTime));
        seq.AppendCallback(() =>
        {
            renderImage.color = new Color(1, 1, 1, 0);
            descriptionImage.color = new Color(1, 1, 1, 0);
            if (tutorialData[curIndex].descriptionImage == null) 
            {
                videoPlayer.clip = tutorialData[curIndex].videoClip;
                videoPlayer.Prepare();
                videoPlayer.Play();
            }
            else 
            {
                descriptionImage.sprite = tutorialData[curIndex].descriptionImage;
                videoPlayer.Stop();
            }
            

           // videoPlayer.Play();
            descriptionText.text = tutorialData[curIndex].descriptionText;
            keywordImage.sprite = tutorialData[curIndex].keywordImage;
            if (curIndex > 0)
            {
                pageMarks[curIndex - 1].color = Color.white;
            }
            if(curIndex+1 < pageMarks.Count) 
            {
                pageMarks[curIndex + 1].color = Color.white;
            }
            pageMarks[curIndex].color = pageMarkColor;
        });
        seq.Append(innerGroup.DOFade(1, changeAnimTime));
        seq.Append(FadeImage(tutorialData[curIndex].descriptionImage == null));
        seq.OnComplete(() =>
        {
            isPlaying = false;
            prevButton.interactable = true;
            nextButton.interactable = true;
        });
        seq.Play();
    }
    private TweenerCore<Color, Color, ColorOptions> FadeImage(bool isOn) 
    {
        if (isOn) 
        {
            return renderImage.DOFade(1, VIDEO_VISIBLE_TIME);
        }
        return descriptionImage.DOFade(1, VIDEO_VISIBLE_TIME);
    }
    private void UpdatePage(int amount)
    {
        if (isPlaying) 
        {
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.AssignmentKeyword);
        isPlaying = true;
        prevButton.interactable = false;
        nextButton.interactable = false;
        curIndex+= amount;
        SetTutorial();
    }
    public void Close() 
    {
        if (isPlaying) 
        {
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.AssignmentKeyword);
        isPlaying = true;
        prevButton.interactable = false;
        nextButton.interactable = false;
        var seq = DOTween.Sequence();
        seq.Append(rootGroup.DOFade(0, openAnimTime));
        seq.Join(clickNotice.DOAnchorPos(startPos, openAnimTime));
        seq.OnComplete(() =>
        {
            Managers.Game.RetryPanel.OpenResetButton();
            Managers.Game.SetStateNormal();
            rootGroup.interactable = false;
            onComplete?.Invoke();
        });
        seq.Play();
    }


    public void ClearGuIdEvent() 
    {
        Managers.Data.ClearEvent(guId);
    }
}
