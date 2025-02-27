using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;


public class CutSceneController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector curCutScene;
    [SerializeField]
    private List<int> cutSceneTalk = new List<int>();
    [SerializeField]
    private CharacterInfo left;
    [SerializeField]
    private CharacterInfo right;

    [SerializeField]
    private UnityEvent onTalkStart;
    [SerializeField]
    private UnityEvent allTalkComplete;

    private int talkCount = 0;
    private bool cutSceneprogress = false;    
    
    public void LoadTalk()
    {
        cutSceneprogress = false;
        
        if(talkCount >= cutSceneTalk.Count)
        {
            talkCount = 0;
        }

        curCutScene.Pause();
        var talk = Managers.Talk.GetTalkEvent(cutSceneTalk[talkCount]);


        talk.OnStart(() => talk.iscutScene = true);
        talk.OnStart(() => talk.left = left);
        talk.OnStart(() => talk.right = right);

        talk.OnComplete(() => talkCount++);
        //재시작        
        //talk.OnComplete(() => SkipCutScene()); //cutScene skip key input
        talk.OnComplete(() => Managers.Game.SetStateEvent());
        talk.OnComplete(() => curCutScene.Resume());
        if(talkCount == cutSceneTalk.Count - 1) 
        {
            talk.OnComplete(() => allTalkComplete?.Invoke());
        }
        //Talk Event Start
        Managers.Talk.PlayCurrentSceneTalk(cutSceneTalk[talkCount]);
    }

    public void LoadBaseTalk()
    {
        cutSceneprogress = false;
        if(talkCount >= cutSceneTalk.Count)
        {
            talkCount = 0;            
        }

        curCutScene.Pause();


        var talk = Managers.Talk.GetTalkEvent(cutSceneTalk[talkCount]);
        talk.OnStart(() => onTalkStart?.Invoke());

        talk.OnComplete(() => talkCount++);
        talk.OnComplete(() => Managers.Game.SetStateEvent());
        talk.OnComplete(() => curCutScene.Resume());

        if (talkCount == cutSceneTalk.Count - 1)
        {
            talk.OnComplete(() => allTalkComplete?.Invoke());
        }
        Managers.Talk.PlayCurrentSceneTalk(cutSceneTalk[talkCount]);
    }

    public void CutScenePause()
    {
        curCutScene.Pause();
    }

    public void SkipCutScene()
    {
        cutSceneprogress = true;
        StartCoroutine(InputSkipKey());
    }

    public void CutSceneComplete()
    {
        CutSceneEvent.isComplete = true;
    }

    IEnumerator InputSkipKey()
    {
        while(cutSceneprogress)
        {
            //Input ESC key
            if (Input.GetKeyDown(KeyCode.Escape))
            {                
                JumpToTime();
                break;
            }
            //CutSceneEnd
            if(curCutScene.state != PlayState.Playing)
            {
                break;
            }
            yield return null;
        }        
    }

    private void JumpToTime()
    {
        curCutScene.Pause();
        curCutScene.time = curCutScene.duration - 1.0f;
        curCutScene.Evaluate();
        curCutScene.Resume();
    }
}
