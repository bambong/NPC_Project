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
    public void LoadTalk()
    {
        curCutScene.Pause();
        var talk = Managers.Talk.GetTalkEvent(cutSceneTalk[talkCount]);
        talk.OnStart(() => Managers.Game.Player.SetstateStop());
        talk.OnComplete(() => Managers.Game.Player.SetStateIdle());
        talk.OnComplete(() => talkCount++);
        //Àç½ÃÀÛ
        talk.OnComplete(() => curCutScene.Resume());

        //Talk Event Start
        Managers.Talk.PlayCurrentSceneTalk(cutSceneTalk[talkCount]);
    }
}
