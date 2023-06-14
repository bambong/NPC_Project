using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField]
    private UnityEvent onComplete;

    private CutSceneEvent cutScene;
    

    public override void OnEventTrigger(Collider other)
    {
        if(startposition != null)
        {
            cutsceneplayer.SetStartPoint(startposition, isLeft);
        }
        base.OnEventTrigger(other);
        StartCoroutine(WaitForSceneLoad());
    }    
    IEnumerator WaitForSceneLoad() 
    {
        while (Managers.Scene.IsTransitioning) 
        {
            yield return null;
        }

        cutScene = new CutSceneEvent(playableDirector);
        cutScene.Play();
        cutScene.OnComplete(() => { onComplete?.Invoke(); });
        
    }
}
