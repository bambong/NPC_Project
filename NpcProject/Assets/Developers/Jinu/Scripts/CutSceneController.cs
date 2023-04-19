using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class CutSceneController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector curCutScene;
    // Update is called once per frame
    [SerializeField]
    private List<int> cutSceneTalk = new List<int>();
    private int talkCount = 0;
    private bool cutSceneprogress = false;
    public void LoadTalk()
    {
        cutSceneprogress = false;

        curCutScene.Pause();
        var talk = Managers.Talk.GetTalkEvent(cutSceneTalk[talkCount]);
        talk.OnComplete(() => talkCount++);
        //Àç½ÃÀÛ
        talk.OnComplete(() => curCutScene.Resume());
        talk.OnComplete(() => SkipCutScene());

        //Talk Event Start
        Managers.Talk.PlayCurrentSceneTalk(cutSceneTalk[talkCount]);
    }

    public void SkipCutScene()
    {
        cutSceneprogress = true;
        StartCoroutine(InputSkipKey());
    }

    IEnumerator InputSkipKey()
    {
        while(cutSceneprogress == true)
        {
            //Input ESC key
            if (Input.GetKeyDown(KeyCode.Escape))
            {                
                JumpToTime();
                break;
            }
            //cutSceneEnd
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
        //curCutScene.time = curCutScene.duration - 1.0f;        
        curCutScene.Evaluate();
        curCutScene.Resume();
    }
}
