using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineTrigger : MonoBehaviour
{
    [SerializeField]
    PlayableDirector playableDirector;

    CutSceneEvent cutScene;

    private bool isPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayed)
        {
            cutScene = new CutSceneEvent(playableDirector);
            cutScene.Play();
            isPlayed = true;
        }
    }
}
