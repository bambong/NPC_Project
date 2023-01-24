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
    public Speaker speaker;
    public string text;
}

public class TalkEvent : GameEvent
{
    private List<Dialogue> dialogues = new List<Dialogue>();
    private int curIndex = 0;

    public TalkEvent() 
    {
        curIndex = 0;
    }
    public override void Play()
    {
        base.Play();
        Managers.Talk.PlayTalk(this);
    }
    public void AddDialogue(Dialogue dialogue)
    {
        dialogues.Add(dialogue);
    }
    public Dialogue GetCurrentDialogue() => dialogues[curIndex];
    public bool MoveNext() 
    {
        curIndex++;

        if (dialogues.Count <= curIndex)
        {
            curIndex = 0;
            Managers.Talk.EndTalk();
            onComplete?.Invoke();
            return false;
        }
           
        return true;
    }
}

public class TalkManager
{
    [SerializeField]
    private TalkPanelController talkPanel;
    private TalkEvent curTalkEvent;

    private readonly float ENTER_ANIM_SPEED = 0.5f;

    private Dictionary<int, TalkEvent> currentSceneTalkDatas = new Dictionary<int, TalkEvent>();
    public Dictionary<int, Speaker> speakerDatas = new Dictionary<int, Speaker>();

    public void Init()
    {
        talkPanel = Managers.UI.MakeSceneUI<TalkPanelController>(null, "DialoguePanel");
        LoadSpeakerData();
    }
    private void LoadSpeakerData() 
    {
        var speakers =  Resources.LoadAll<Speaker>("Data/SpeakerData/");

        for(int i =0; i< speakers.Length; ++i)
        {
            speakerDatas.Add(speakers[i].Id,speakers[i]);
        }
    }
    public void LoadTalkData(DialogueExcel dialogueExcel) 
    {
        currentSceneTalkDatas.Clear();

        for(int i = 0; i < dialogueExcel.datas.Count; ++i) 
        {
            var data = dialogueExcel.datas[i];

            TalkEvent talkEvent;
            Dialogue dialogue = new Dialogue();
            dialogue.text = data.text;
            dialogue.speaker = speakerDatas[data.speaker];

            if(!currentSceneTalkDatas.TryGetValue(data.eventID, out talkEvent))
            {
                talkEvent = new TalkEvent();
                currentSceneTalkDatas.Add(data.eventID, talkEvent);
            }
      
            talkEvent.AddDialogue(dialogue);

        }
    }
    public void PlayDialogue(Dialogue dialogue) 
    {
        talkPanel.PlayDialogue(dialogue);
    }
    public TalkEvent GetTalkEvent(int talkEventId) 
    {
        return currentSceneTalkDatas[talkEventId];
    }
 
    public void PlayTalk(TalkEvent talk) 
    {
        Managers.Game.SetStateDialog();
        
        curTalkEvent = talk;
       
        talkPanel.SetDialogue(curTalkEvent.GetCurrentDialogue());

        talkPanel.TalkPanelInner.DOScale(Vector3.zero, 0).OnStart(() =>
        {
            talkPanel.gameObject.SetActive(true);
            talkPanel.TalkPanelInner.DOScale(Vector3.one, ENTER_ANIM_SPEED).OnComplete(() =>
            {
                Managers.Scene.CurrentScene.StartCoroutine(ProgressTalk());
            });
        });
    }
    public void PlayCurrentSceneTalk(int talkEventId)
    {
        GetTalkEvent(talkEventId).Play();
    }
    public void EndTalk() 
    {
        talkPanel.gameObject.SetActive(false);
        Managers.Game.SetStateNormal();
    }
    public void Clear() 
    {
        currentSceneTalkDatas.Clear();
    }
    IEnumerator ProgressTalk() 
    {

        PlayDialogue(curTalkEvent.GetCurrentDialogue());
        
        while(true) 
        {
            if(Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.TALK_KEY)) && talkPanel.IsNext == true) 
            {
                if(curTalkEvent.MoveNext())
                {
                    PlayDialogue(curTalkEvent.GetCurrentDialogue());
                }
                else 
                {
                    break;
                }
            }
            yield return null;
        }
    }

    
}
