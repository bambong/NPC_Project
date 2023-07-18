using DG.Tweening;
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
    public VideoClip videoClip;
    [TextArea(2, 10)]
    public string descriptionText;
}

public class TutorialDescriptionPanelController : BasePanelController
{
    [SerializeField]
    private PausePanelController pausePanel;

    [SerializeField]
    private CanvasGroup innerGroup;
    [SerializeField]
    private RawImage renderImage;

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
        videoPlayer.Prepare();
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        renderImage.color = Color.white;
      
        videoPlayer.Play();
    }
    public void SetPageUpdate() 
    {
        //Time.timeScale = 1;
        videoPlayer.clip = currentData.pauseTutorialPartDatas[currentIndex].videoClip;
        descriptionText.text = currentData.pauseTutorialPartDatas[currentIndex].descriptionText;
    
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
            renderImage.color = new Color(1,1,1,0);
            SetPageUpdate();
          
        });
        seq.Append(innerGroup.DOFade(1, TRANSITION_TIME / 2));
        seq.AppendCallback(() => {
            videoPlayer.Prepare();
            videoPlayer.Play();
        });
        seq.AppendInterval(0.2f);
        seq.Append(renderImage.DOFade(1, 0.5f));
        seq.OnComplete(() =>
        {
            isTransition = false;
        }
        );
        seq.Play();
    }
}
