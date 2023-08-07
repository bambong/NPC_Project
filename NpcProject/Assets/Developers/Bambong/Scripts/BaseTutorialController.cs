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
public class TutorialData
{
    public Sprite descriptionImage;
    public VideoClip videoClip;
    public Sprite keywordImage;
    [Header("제목")]
    public string title;
    [Header("설명")]
    [TextArea(2, 10)]
    public string descriptionText;
}

public class BaseTutorialController : GuIdBehaviour
{

    [Header("튜토리얼 데이터")]
    [SerializeField]
    protected List<TutorialData> tutorialData;
   
    [Space(3)]
    [Header("시작/종료 이벤트")]
    [SerializeField]
    protected UnityEvent onStart;
    [SerializeField]
    protected UnityEvent onComplete;

    
    [Header("비디오")]
    [SerializeField]
    protected VideoPlayer videoPlayer;
    [Space(3)]
    [Header("레이아웃 그룹")]
    [SerializeField]
    protected CanvasGroup rootGroup;
    [SerializeField]
    protected CanvasGroup innerGroup;

    [Space(3)]
    [Header("버튼")]
    [SerializeField]
    protected Button nextButton;
    [SerializeField]
    protected Button prevButton;
    [SerializeField]
    protected Button exitButton;

    [Space(3)]
    [Header("출력용")]
    [SerializeField]
    protected Image keywordImage;
    [SerializeField]
    protected RawImage renderImage;
    [SerializeField]
    protected Image descriptionImage;
    [SerializeField]
    protected TextMeshProUGUI titleText;
    [SerializeField]
    protected TextMeshProUGUI descriptionText;

    [Space(3)]
    [Header("페이지 위치 마커 관련")]
    [SerializeField]
    protected List<Image> pageMarks;
    [SerializeField]
    protected Color pageMarkColor;
    
    [Space(3)]
    [Header("클릭하여 진행 UI 관련")]
    [SerializeField]
    protected RectTransform clickNotice;
    [SerializeField]
    protected Vector3 startPos;
    [SerializeField]
    protected Vector3 desirePos;

    [Space(3)]
    [Header("오브젝트가 켜지면 자동 실행")]
    [SerializeField]
    protected bool enableStart = true;
    [Header("시작 시간 딜레이")]
    [SerializeField]
    protected float startDelay;

    [Header("처음 페이지 시작 애니메이션 시간")]
    [SerializeField]
    protected float openAnimTime = 0.5f;
    [Header("다른 페이지 변경 애니메이션 시간")]
    [SerializeField]
    protected float changeAnimTime = 0.3f;

    protected bool isOpen;
    protected int curIndex;
    protected bool isPlaying;
    protected const float VIDEO_VISIBLE_TIME = 0.2f;


    protected override void Start()
    {
        base.Start();

        for (int i = tutorialData.Count; i < pageMarks.Count; ++i)
        {
            pageMarks[i].gameObject.SetActive(false);
        }
        exitButton.onClick.AddListener(()=>Close());
        clickNotice.anchoredPosition = startPos;
        rootGroup.alpha = 0;
        innerGroup.alpha = 0;
        renderImage.color = new Color(1, 1, 1, 0);
        descriptionImage.color = new Color(1, 1, 1, 0);
        if (enableStart)
        {
            Open();
        }
    }
    public virtual bool Open() 
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
        seq.OnComplete(SetTutorial);
        seq.Play();
        return true;
    }
    public virtual bool Close()
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
    protected virtual void OnOpen() 
    {
    
    }
    protected virtual void OnClose()
    {

    }

    protected void SetTutorial()
    {
        isPlaying = true;
        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();

        var seq = DOTween.Sequence();
        seq.Append(innerGroup.DOFade(0, changeAnimTime));
        seq.AppendCallback(() =>
        {
            if (curIndex > 0)
            {
                prevButton.gameObject.SetActive(true);
                prevButton.onClick.AddListener(() => UpdatePage(-1));
            }
            else
            {
                prevButton.gameObject.SetActive(false);
            }

            if (curIndex >= tutorialData.Count - 1)
            {
                nextButton.gameObject.SetActive(false);
                exitButton.gameObject.SetActive(true);
            }
            else
            {
                nextButton.gameObject.SetActive(true);
                exitButton.gameObject.SetActive(false);
                nextButton.onClick.AddListener(() => UpdatePage(+1));
            }


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

            titleText.text = tutorialData[curIndex].title;
            descriptionText.text = KeywordTextParse(tutorialData[curIndex].descriptionText);
            keywordImage.sprite = tutorialData[curIndex].keywordImage;
            if (curIndex > 0)
            {
                pageMarks[curIndex - 1].color = Color.white;
            }
            if (curIndex + 1 < pageMarks.Count)
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

    private string KeywordTextParse(string str)
    {
        char[] sep = { '#', '#' };

        string[] result = str.Split(sep);
        string resultText = "";

        foreach (var item in result)
        {
            switch (item)
            {
                case "debugmode":
                    resultText += "[" + KeySetting.Instance.currentKeys[KEY_TYPE.DEBUGMOD_KEY].ToString() + "]";
                    break;
                case "player":
                    resultText += Managers.Talk.GetSpeakerName(101);
                    break;

                default:
                    resultText += item;
                    break;
            }
        }
        return resultText;
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
        curIndex += amount;
        SetTutorial();
    }
}
