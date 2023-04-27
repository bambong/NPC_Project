using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DynamicTimelineBinding : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public PlayableAsset playableAsset;
    public List<GameObject> newTarget = new List<GameObject>();

    private void Start()
    {
        //Binding();
    }
    public void Binding()
    {
        var outputs = playableAsset.outputs;
        foreach (var item in outputs)
        {
            Debug.Log(item.streamName);
            if (item.streamName == "Transform Tween Track")
            {
                playableDirector.SetGenericBinding(item.sourceObject, newTarget[0].gameObject.transform);
            }
        }
    }
}
