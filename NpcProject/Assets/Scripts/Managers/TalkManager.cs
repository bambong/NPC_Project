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
    private int curIndex = 0;
    public bool MoveNext(out Dialogue dialogue)
    {
        bool result = false;
        if(curIndex <= dialogues.Count -1) 
        {
            curIndex = curIndex % dialogues.Count;
            result = true;
        }
        var temp = dialogues[curIndex];
        curIndex++;
        dialogue = temp;
        return result;
    }
}

public class Talk : ScriptableObject
{
    public int Id;
    public List<Speak> talks;
    private int curIndex = 0;
    public bool MoveNext(out Speak speak)
    {
        bool result = false;
        if(curIndex <= talks.Count - 1)
        {
            curIndex = curIndex % talks.Count;
            result = true;
        }
        curIndex = curIndex % talks.Count;
        var temp = talks[curIndex];
        curIndex++;
        speak = temp;
        return result; 
    }
}

public class TalkManager : GameObjectSingleton<TalkManager>, IInit
{
    [SerializeField]
    private TalkPanelController talkPanel;

    private Talk curTalk;
    public void Init()
    {
        LoadTalkData();   
    }
    private void LoadTalkData() 
    {

    }
    public void EnterTalk(Talk talk) 
    {
        curTalk = talk;
       // Speak speak;
       // talkPanel.SetText();

    }
    public void ProgressTalk() 
    {
        Speak speak;
        if(curTalk.MoveNext(out speak)) 
        {
            // 대화 종료
        }
        ProgressSpeak();
    }
    public void ProgressSpeak() 
    {
        if(talkPanel.MoveNext())
        {
            ProgressTalk();
        }
    }
   

}
