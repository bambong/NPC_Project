using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : EventTrigger
{
    [SerializeField]
    PlayableDirector playableDirector;

    CutSceneEvent cutScene;

    public override void OnEventTrigger(Collider other)
    {
        cutScene = new CutSceneEvent(playableDirector);
        cutScene.Play();
        base.OnEventTrigger(other);
    }
}
