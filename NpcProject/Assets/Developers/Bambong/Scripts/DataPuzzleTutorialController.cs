using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

[Serializable]
public class DataPuzzleTutorialData 
{
    public Sprite descriptionImage;
    [TextArea(2,10)]
    public string descriptionText;
}

public class DataPuzzleTutorialController : GuIdBehaviour
{

    [SerializeField]
    private CanvasGroup rootGroup;

    [SerializeField]
    private CanvasGroup innerGroup;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private HelpButtonController helpButton;

    [SerializeField]
    private Image descriptionImage;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private List<DataPuzzleTutorialData> tutorialData;

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

    private int curIndex;
    private bool isPlaying;
    private bool isOpen;
    private const float IMAGE_VISIBLE_TIME = 0.3f;
    protected override void Start()
    {
        for ( int i = tutorialData.Count; i < pageMarks.Count; ++i) 
        {
            pageMarks[i].gameObject.SetActive(false);
        }
        rootGroup.blocksRaycasts = false;
        rootGroup.alpha = 0;
        innerGroup.alpha = 0;
        helpButton.Init(Open);
    }



    public void Open()
    {
        if (isOpen) 
        {
            return;
        }

        isOpen = true;
        curIndex = 0;
        rootGroup.alpha = 0;
        innerGroup.alpha = 0;

        rootGroup.interactable = true;
        rootGroup.blocksRaycasts = true;
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            onStart?.Invoke();
        });
        seq.Append(rootGroup.DOFade(1, openAnimTime));
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
            descriptionImage.sprite = tutorialData[curIndex].descriptionImage;
            descriptionImage.color = new Color(1,1,1,0);
            descriptionText.text = tutorialData[curIndex].descriptionText;
        });
        seq.Append(innerGroup.DOFade(1, changeAnimTime));
        seq.AppendCallback(() => 
        {
            for (int i = 0 ; i < pageMarks.Count; ++i)
            {
                if(curIndex == i)
                {
                    pageMarks[i].color = pageMarkColor;
                    continue;
                }
                pageMarks[i].color = Color.white;
            }
            helpButton.SetCloseMod(Close, changeAnimTime);
        });
        seq.Append(descriptionImage.DOFade(1, IMAGE_VISIBLE_TIME));
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
        if (isPlaying || !isOpen) 
        {
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.AssignmentKeyword);
        isPlaying = true;
        nextButton.interactable = false;

        helpButton.SetOpenMod(Open, openAnimTime);
        var seq = DOTween.Sequence();
        seq.Append(rootGroup.DOFade(0, openAnimTime));
        seq.OnComplete(() =>
        {
            rootGroup.blocksRaycasts = false;
            rootGroup.interactable = false;
            isOpen = false;
            onComplete?.Invoke();
        });
        seq.Play();
    }
  

}
