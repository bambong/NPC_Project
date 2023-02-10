using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class MoveTutorialController : MonoBehaviour, IInteraction
{

    [SerializeField]
    private Transform spawnSpot;

    [SerializeField]
    private MMFeedbacks feedbacks;
    public GameObject Go => gameObject;

    public void OnInteraction()
    {
        feedbacks.PlayFeedbacks();
        Managers.Game.Player.transform.position = spawnSpot.position;
        Managers.Game.Player.SetStateIdle();
    }
}
