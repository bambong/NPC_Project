using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
[Serializable]
public class Dialogue 
{
    public string text;
}

[Serializable]
public class Speak
{
    public string speakerName;
    public Sprite speakerImage;
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

    private TalkEvent curTalkEvent;
    private readonly WaitForSeconds INPUT_CHECK_WAIT = new WaitForSeconds(0.05f);
    public void Init()
    {
        LoadTalkData();   
    }
    private void LoadTalkData() 
    {
    }
    public void EnterTalk(Talk talk) 
    {
        GameManager.Instance.SetStateDialog();
        curTalkEvent = new TalkEvent(talk,talkPanel);
        talkPanel.gameObject.SetActive(true);
        
        StartCoroutine(ProgressTalk());
    }
    private void EndTalk() 
    {
        talkPanel.gameObject.SetActive(false);
        GameManager.Instance.SetStateNormal();
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
            if(Input.GetKeyDown(KeyCode.X)) 
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
