using DG.Tweening;
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
    private Image keywordImage;
    [SerializeField]
    private RawImage renderImage;

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
    private const float VIDEO_VISIBLE_TIME = 0.3f;
    protected override void Start()
    {
        for ( int i = tutorialData.Count; i < pageMarks.Count; ++i) 
        {
            pageMarks[i].gameObject.SetActive(false);
        }
        clickNotice.anchoredPosition = startPos;
        rootGroup.alpha = 0;
        innerGroup.alpha = 0;
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
        Managers.Game.Player.SetstateStop();
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
        if (curIndex >= tutorialData.Count - 1) 
        {
            nextButton.onClick.AddListener(Close);
        }
        else 
        {
            nextButton.onClick.AddListener(UpdateNextPage);
        }
        var seq = DOTween.Sequence();
        seq.Append(innerGroup.DOFade(0, changeAnimTime));
        seq.AppendCallback(() =>
        {
            videoPlayer.clip = tutorialData[curIndex].videoClip;
            renderImage.color = new Color(1,1,1,0);
            videoPlayer.Prepare();
           // videoPlayer.Play();
            descriptionText.text = tutorialData[curIndex].descriptionText;
            keywordImage.sprite = tutorialData[curIndex].keywordImage;
            videoPlayer.Play();
        });
        seq.Append(innerGroup.DOFade(1, changeAnimTime));
        seq.AppendCallback(() => 
        {
            if (curIndex > 0)
            {
                pageMarks[curIndex - 1].DOColor(Color.white, 0.2f);
            }
        });
        seq.Join(pageMarks[curIndex].DOColor(pageMarkColor, 0.2f));
        seq.Append(renderImage.DOFade(1, VIDEO_VISIBLE_TIME));
        seq.OnComplete(() =>
        {
            isPlaying = false;
          
            nextButton.interactable = true;
        });
        seq.Play();
    }
    private void UpdateNextPage()
    {
        if (isPlaying) 
        {
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.AssignmentKeyword);
        isPlaying = true;
        nextButton.interactable = false;
        ++curIndex;
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
        nextButton.interactable = false;
        var seq = DOTween.Sequence();
        seq.Append(rootGroup.DOFade(0, openAnimTime));
        seq.Join(clickNotice.DOAnchorPos(startPos, openAnimTime));
        seq.OnComplete(() =>
        {
            Managers.Game.RetryPanel.OpenResetButton();
            Managers.Game.Player.SetStateIdle();
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
