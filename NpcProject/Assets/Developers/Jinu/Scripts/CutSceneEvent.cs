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
        Managers.Scene.CurrentScene.StartCoroutine(CutSceneLoad());
        onStart?.Invoke();
    }

    public void Complete()
    {
        onComplete?.Invoke();
    }

    private IEnumerator CutSceneLoad()
    {
        float alpha = 0;
        float progress = 0;
        while (progress < 3.0f)
        {
            progress += Time.deltaTime;
            Managers.Scene.sceneTransition.canvasGroup.alpha = Mathf.Lerp(alpha, 1, progress);
            yield return null;
        }
        Managers.Scene.sceneTransition.canvasGroup.alpha = alpha;
        timelineDirector.Play();
        Managers.Scene.CurrentScene.StartCoroutine(CutSceneUnload());
    }

    private IEnumerator CutSceneUnload()
    {
        while(true)
        {
            if(!(timelineDirector.state == PlayState.Playing))
            {
                float alpha = 1;
                float progress = 0;

                while (progress < 1)
                {
                    progress += Time.deltaTime;
                    Managers.Scene.sceneTransition.canvasGroup.alpha = Mathf.Lerp(alpha, 0, progress);
                    yield return null;
                }
                Managers.Scene.sceneTransition.canvasGroup.alpha = 0;
                Complete();
                yield break;
            }
            yield return null;
        }        
    }

}
