using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffect : MonoBehaviour
{
    [SerializeField]
    protected MMF_Player feedbacks;

    public virtual void Play(Transform targetTrs) 
    {
        transform.position = targetTrs.transform.position;
        feedbacks.PlayFeedbacks();
    }
    public void EffectRemove() 
    {
        Managers.Resource.Destroy(gameObject);  
    }
}
