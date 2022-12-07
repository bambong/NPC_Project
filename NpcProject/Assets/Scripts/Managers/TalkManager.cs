using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Cinemachine;

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

public class TalkManager 
{
    [SerializeField]
    private TalkPanelController talkPanel;
    private CinemachineVirtualCamera talkVircam = null;
    private TalkEvent curTalkEvent;
    private readonly WaitForSeconds INPUT_CHECK_WAIT = new WaitForSeconds(0.01f);
    public void Init()
    {
        LoadTalkData();
        talkPanel = Managers.UI.ShowPopupUI<TalkPanelController>("DialoguePanel");
    }
    private void LoadTalkData() 
    {
    }
    public void EnterTalk(Talk talk, CinemachineVirtualCamera virCam)
    {
        Managers.Game.SetStateDialog();
        curTalkEvent = new TalkEvent(talk,talkPanel);
        talkPanel.gameObject.SetActive(true);
       // CameraSet(virCam);
       // EnterCamera(talkVircam);

       Managers.Scene.CurrentScene.StartCoroutine(ProgressTalk());
    }
    private void EndTalk() 
    {
        talkPanel.gameObject.SetActive(false);
        Managers.Game.SetStateNormal();
       // ExitCamera(talkVircam);
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
            virCam.gameObject.SetActive(true);
        }
    }

    private void ExitCamera(CinemachineVirtualCamera virCam)
    {
        if (talkPanel.gameObject.activeSelf == false)
        {
            virCam.gameObject.SetActive(false);
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
            yield return null;
        }
    }

    
}
