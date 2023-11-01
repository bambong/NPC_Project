using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class PlayTimeline : GuIdBehaviour
{
    [SerializeField]
    private PlayableDirector playableDirector;
    [SerializeField]
    private UnityEvent onStart;
    [SerializeField]
    private UnityEvent onComplete;

    private void PlayEvent()
    {
        var cutScene = new CutSceneEvent(playableDirector);        
        cutScene.Play();
        cutScene.OnStart(() => { onStart?.Invoke(); });
        cutScene.OnComplete(() => { onComplete?.Invoke(); });
    }
}
