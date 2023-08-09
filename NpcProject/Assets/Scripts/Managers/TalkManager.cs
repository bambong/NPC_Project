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
    public Speaker[] leftRights = new Speaker[2];
    public string text;
}

public class TalkEvent : GameEvent
{
    private List<Dialogue> dialogues = new List<Dialogue>();
    private int curIndex = 0;
    public bool iscutScene = false;

    public CharacterInfo left;
    public CharacterInfo right;

    public TalkEvent() 
    {
        curIndex = 0;
    }
    public override void Play()
    {
        onStart?.Invoke();
        if(iscutScene)
        {
            Managers.Talk.PlayCutSceneTalk(this, left, right);
        }
        else
        {
            Managers.Talk.PlayTalk(this);
        }
        
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
    public void MoveEnd()
    {        
        curIndex = 0;
        Managers.Talk.StopTextAnimation();
        Managers.Talk.EndTalk();        
        onComplete?.Invoke();
    }
}

public class TalkManager
{
    [SerializeField]
    private TalkPanelController talkPanel;
    private TalkEvent curTalkEvent;

    private readonly float ENTER_ANIM_SPEED = 0.5f;

    private Dictionary<int, TalkEvent> currentSceneTalkDatas = new Dictionary<int, TalkEvent>();
    private Dictionary<int, Speaker> speakerDatas = new Dictionary<int, Speaker>();
    private const int PARTICIPANT_NUM = 1000;
    public void Init()
    {
        talkPanel = Managers.UI.MakeSceneUI<TalkPanelController>(null, "DialoguePanel");
        LoadSpeakerData();
    }
    public void OnSceneLoaded()
    {
        talkPanel = Managers.UI.MakeSceneUI<TalkPanelController>(null, "DialoguePanel");
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
            if (!speakerDatas.ContainsKey(data.speaker)) 
            {
                continue;
            }
            TalkEvent talkEvent;
            Dialogue dialogue = new Dialogue();
            dialogue.text = data.text;
            dialogue.speaker = speakerDatas[data.speaker];
            if(data.leftRight/ PARTICIPANT_NUM != 0) 
            {
                dialogue.leftRights[0] = speakerDatas[data.leftRight / PARTICIPANT_NUM];
            }
            if (data.leftRight % PARTICIPANT_NUM != 0)
            {
                dialogue.leftRights[1] = speakerDatas[data.leftRight % PARTICIPANT_NUM];
            }

            if (!currentSceneTalkDatas.TryGetValue(data.eventID, out talkEvent))
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
        Managers.Game.SetStateEvent();
        talkPanel.skipButton.gameObject.SetActive(false);

        curTalkEvent = talk;
       
        talkPanel.SetDialogue(curTalkEvent.GetCurrentDialogue());

        talkPanel.TalkPanelInner.DOScale(Vector3.zero, 0).OnStart(() =>
        {
            talkPanel.gameObject.SetActive(true);
            talkPanel.TalkPanelInner.DOScale(Vector3.one, ENTER_ANIM_SPEED).OnComplete(() =>
            {
                talkPanel.skipButton.gameObject.SetActive(true);
                Managers.Scene.CurrentScene.StartCoroutine(ProgressTalk());
            });
        });
    }

    public string GetSpeakerName(int id)
    {
        string name = speakerDatas[id].charName;
        return name;
    }

    public void PlayCurrentSceneTalk(int talkEventId)
    {
        GetTalkEvent(talkEventId).Play();
    }
    public void EndTalk() 
    {
        talkPanel.RestorationMat();
        talkPanel.gameObject.SetActive(false);
        Managers.Game.SetStateNormal();
    }
    public void StopTextAnimation()
    {
        talkPanel.StopTextAnimation();
    }
    public void Clear() 
    {
        currentSceneTalkDatas.Clear();
    }

    public void SkipDialogue()
    {
        talkPanel.PanelClear();
        curTalkEvent.MoveEnd();
    }
    IEnumerator ProgressTalk() 
    {

        PlayDialogue(curTalkEvent.GetCurrentDialogue());
        
        while(true) 
        {
            if (Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.EXIT_KEY)))
            {
                talkPanel.PanelClear();
                curTalkEvent.MoveEnd();
                break;
            }

            if(((Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.SKIP_KEY))) && talkPanel.IsNext == true && !talkPanel.IsChoice) || talkPanel.GetIsSelect()) 
            {
                talkPanel.InputIsSelect(false);

                if(curTalkEvent.MoveNext())
                {
                    talkPanel.SaveText();
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

    #region CutSceneTalk Function
    public void PlayDialogue(Dialogue dialogue, CharacterInfo left, CharacterInfo right)
    {
        talkPanel.PlayDialogue(dialogue, left, right);
    }

    public void PlayCutSceneTalk(TalkEvent talk, CharacterInfo left, CharacterInfo right)
    {
        Managers.Game.SetStateEvent();

        curTalkEvent = talk;

        talkPanel.SetDialogue(curTalkEvent.GetCurrentDialogue(), left, right);

        talkPanel.TalkPanelInner.DOScale(Vector3.zero, 0).OnStart(() =>
        {
            talkPanel.gameObject.SetActive(true);
            talkPanel.TalkPanelInner.DOScale(Vector3.one, ENTER_ANIM_SPEED).OnComplete(() =>
            {
                Managers.Scene.CurrentScene.StartCoroutine(ProgresscutSceneTalk(left, right));
            });
        });
    }

    IEnumerator ProgresscutSceneTalk(CharacterInfo left, CharacterInfo right)
    {

        PlayDialogue(curTalkEvent.GetCurrentDialogue(), left, right);

        while (true)
        {
            if (Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.EXIT_KEY)))
            {
                curTalkEvent.MoveEnd();
                break;
            }

            if (((Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.SKIP_KEY))) && talkPanel.IsNext == true && !talkPanel.IsChoice) || talkPanel.GetIsSelect())
            {
                talkPanel.InputIsSelect(false);

                if (curTalkEvent.MoveNext())
                {
                    talkPanel.SaveText();
                    PlayDialogue(curTalkEvent.GetCurrentDialogue(), left, right);
                }
                else
                {
                    break;
                }
            }
            yield return null;
        }
    }
    #endregion
}
