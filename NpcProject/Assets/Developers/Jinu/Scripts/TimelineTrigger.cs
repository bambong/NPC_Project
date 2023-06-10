using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : EventTrigger
{
    [SerializeField]
    private PlayableDirector playableDirector;
    [SerializeField]
    private Transform startposition;
    [SerializeField]
    private CutScenePlayerController cutsceneplayer;
    [SerializeField]
    private bool isLeft;
    private CutSceneEvent cutScene;
    

    public override void OnEventTrigger(Collider other)
    {
        cutsceneplayer.SetStartPoint(startposition, isLeft);

        cutScene = new CutSceneEvent(playableDirector);
        cutScene.Play();
        base.OnEventTrigger(other);
    }    
}
