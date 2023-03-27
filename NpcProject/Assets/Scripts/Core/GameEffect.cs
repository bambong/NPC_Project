using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffect : MonoBehaviour
{
    [SerializeField]
    protected MMF_Player feedbacks;

    private void Awake()
    {
        //feedbacks.Initialization();
    }
    public virtual void Play(Transform targetTrs) 
    {
        feedbacks.PlayFeedbacks();
    }
    public void EffectRemove() 
    {
        Managers.Resource.Destroy(gameObject);  
    }
}
