using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
public class CutSceneEvent : GameEvent
{
    private PlayableDirector timelineDirector;

    public CutSceneEvent(PlayableDirector timeline)
    {
        timelineDirector = timeline;
    }


    public override void Play()
    {
        onStart?.Invoke();
        timelineDirector.Play();
        Managers.Scene.CurrentScene.StartCoroutine(CutSceneUnload());
    }

    public void Complete()
    {
        onComplete?.Invoke();
    }

    private IEnumerator CutSceneUnload()
    {
        while(true)
        {
            if(!(timelineDirector.state == PlayState.Playing))
            {
                Complete();
                yield break;
            }
            yield return null;
        }        
    }

}
