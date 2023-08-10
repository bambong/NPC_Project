using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


[Serializable]
public class PauseTutorialData
{
    public Sprite iconImage;
    public string iconTitle;
    public List<PauseTutorialPartData> pauseTutorialPartDatas;
}
[Serializable]
public class PauseTutorialPartData
{
    public string title;
    public Sprite descriptionImage;
    public VideoClip videoClip;
    [TextArea(2, 10)]
    public string descriptionText;
}

public class TutorialDescriptionPanelController : BasePanelController
{
    [SerializeField]
    private PausePanelController pausePanel;

    [SerializeField]
    private Image titleImage;
    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private CanvasGroup innerGroup;
    [SerializeField]
    private RawImage renderImage;
    [SerializeField]
    private Image descriptionImage;


    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private List<TutorialSideButtonController> sideButtonControllers;

    private int currentIndex;
    private PauseTutorialData currentData;
    private bool isTransition;
    
    private readonly float TRANSITION_TIME = 0.5f;

    public void SelectTutorialNode(TutorialNodeController node) 
    {
        currentIndex = 0;
        currentData = node.PauseTutorialData;

        titleImage.sprite = currentData.iconImage;
        titleText.text = currentData.iconTitle;

        for (int i = 0; i < sideButtonControllers.Count; ++i)
        {
            if(i >= currentData.pauseTutorialPartDatas.Count) 
            {
                sideButtonControllers[i].gameObject.SetActive(false);
            }
            else 
            {
                sideButtonControllers[i].gameObject.SetActive(true);
                sideButtonControllers[i].SetInfo(i, currentData.pauseTutorialPartDatas[i]);
                sideButtonControllers[i].OnHighlight(currentIndex == i);
            }
        }
        SetPageUpdate();
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        renderImage.color = Color.white;
      
        if(currentData.pauseTutorialPartDatas[currentIndex].descriptionImage == null) 
        {
            renderImage.color = new Color(1, 1, 1, 1);
        }
        else 
        {
            descriptionImage.color = new Color(1, 1, 1, 1);
        }
        videoPlayer.Play();
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
    public void SetPageUpdate() 
    {
        //Time.timeScale = 1;
        if (currentData.pauseTutorialPartDatas[currentIndex].descriptionImage == null)
        {
            descriptionImage.color = new Color(1, 1, 1, 0);
            videoPlayer.clip = currentData.pauseTutorialPartDatas[currentIndex].videoClip;
            videoPlayer.Prepare();
            videoPlayer.Play();
        }
        else
        {
            renderImage.color = new Color(1, 1, 1, 0);
            descriptionImage.sprite = currentData.pauseTutorialPartDatas[currentIndex].descriptionImage;
            videoPlayer.Stop();
        }
   
        descriptionText.text = KeywordTextParse(currentData.pauseTutorialPartDatas[currentIndex].descriptionText);
    
    }
    public void CurIndexUpdate(int index) 
    {
        currentIndex = index;

        for (int i = 0; i < currentData.pauseTutorialPartDatas.Count; ++i)
        {
            sideButtonControllers[i].OnHighlight(currentIndex == i);
        }
    }

    public void ChangePage(int index)
    {
        if (isTransition)
        {
            return;
        }
        isTransition = true;
        CurIndexUpdate(index);

        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(innerGroup.DOFade(0, TRANSITION_TIME / 2));
        seq.AppendCallback(() =>
        {
            SetPageUpdate();

            renderImage.color = new Color(1, 1, 1, 0);
            descriptionImage.color = new Color(1, 1, 1, 0);

        });
        seq.Append(innerGroup.DOFade(1, TRANSITION_TIME / 2));
        seq.Append(FadeImage(currentData.pauseTutorialPartDatas[currentIndex].descriptionImage == null, 0.3f));
        seq.OnComplete(() =>
        {
            isTransition = false;
        }
        );
        seq.Play();
    }
    private TweenerCore<Color, Color, ColorOptions> FadeImage(bool isOn ,float time)
    {
        if (isOn)
        {
            return renderImage.DOFade(1, time);
        }
        return descriptionImage.DOFade(1, time);
    }
}
