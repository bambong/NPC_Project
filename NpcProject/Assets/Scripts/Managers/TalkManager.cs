using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Cinemachine;
using DG.Tweening;

[Serializable]
public class Dialogue 
{
    public string text;
}

[Serializable]
public class Speak
{
    public Speaker speaker;
    public List<Dialogue> dialogues;
}

public class TalkEvent 
{
    public Talk talk;
    private int curIndex = 0;
    private TalkPanelController talkPanel;
    public TalkEvent(Talk talk ,TalkPanelController talkpanel) 
    {
        this.talk = talk;
        talkPanel = talkpanel;
        talkPanel.SetText(talk.speaks[0]);
    }
    public bool ProgressTalk() 
    {
        while(true) 
        {
            if(!talkPanel.MoveNext())
            {
                curIndex++;

                if(talk.speaks.Count <= curIndex)
                {
                    return false;
                }
                talkPanel.SetText(talk.speaks[curIndex]);
                continue;
            }
            break;
        }
        return true;
    }
}

public class TalkManager : GameObjectSingleton<TalkManager>, IInit
{
    [SerializeField]
    private TalkPanelController talkPanel;
    private CinemachineVirtualCamera talkVircam = null;
    private Image ioImage = null;
    private TalkEvent curTalkEvent;
    private readonly WaitForSeconds INPUT_CHECK_WAIT = new WaitForSeconds(0.01f);
    public void Init()
    {
        LoadTalkData();   
    }
    private void LoadTalkData() 
    {
    }
    public void EnterTalk(Talk talk, CinemachineVirtualCamera virCam)
    {
        GameManager.Instance.SetStateDialog();
      
        curTalkEvent = new TalkEvent(talk,talkPanel);

        talkPanel.gameObject.SetActive(true);
        CutScene(ioImage);
        CameraSet(virCam);
        EnterCamera(talkVircam);

        StartCoroutine(ProgressTalk());
    }

    private void EndTalk() 
    {
        talkPanel.gameObject.SetActive(false);
        GameManager.Instance.SetStateNormal();
        ExitCamera(talkVircam);
        CutScene(ioImage);
    }

    private void CutScene(Image ioImage)
    {
        ioImage = GameObject.Find("Canvas").transform.Find("Camera Cut").GetComponent<Image>();
        ioImage.gameObject.SetActive(true);
        ioImage.DOFade(0, 0.5f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            ioImage.gameObject.SetActive(false);
            ioImage.DOFade(1, 0f);
        }
        );
    }

    private void CameraSet(CinemachineVirtualCamera virCam)
    {
        talkVircam = virCam;
    }

    private void EnterCamera(CinemachineVirtualCamera virCam)
    {
        if (virCam != null)
        {
            Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("Player"));
            virCam.Priority = 100;
            // virCam.gameObject.SetActive(true);
        }
    }

    private void ExitCamera(CinemachineVirtualCamera virCam)
    {
        if (talkPanel.gameObject.activeSelf == false)
        {
            virCam.Priority = 1;
            // virCam.gameObject.SetActive(false);
            Camera.main.cullingMask = -1;
        }
    }

    IEnumerator ProgressTalk() 
    {
        if(!curTalkEvent.ProgressTalk())
        {
            EndTalk();
            yield break;
        }
        while(true) 
        {
            if(Input.GetKeyDown(KeyCode.X) && talkPanel.IsAni() == true) 
            {
                if(!curTalkEvent.ProgressTalk())
                {
                    EndTalk();
                    break;
                }
            }
            yield return INPUT_CHECK_WAIT;
        }
    }

    
}
