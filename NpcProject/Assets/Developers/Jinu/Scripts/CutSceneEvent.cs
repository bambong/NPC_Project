using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
public class CutSceneEvent : GameEvent
{
    static public bool isComplete = false;

    private PlayableDirector timelineDirector;
    public CutSceneEvent(PlayableDirector timeline)
    {
        timelineDirector = timeline;
    }


    public override void Play()
    {
        isComplete = false;
        onStart?.Invoke();
        timelineDirector.Play();
        Managers.Game.SetStateEvent();
        Managers.Scene.CurrentScene.StartCoroutine(CutSceneUnload());
    }

    public void Complete()
    {
        Managers.Game.SetStateNormal();
        onComplete?.Invoke();
    }

    private IEnumerator CutSceneUnload()
    {
        while(true)
        {
            if (isComplete)
            {
                Complete();
                yield break;
            }
            yield return null;
        }        
    }

}
