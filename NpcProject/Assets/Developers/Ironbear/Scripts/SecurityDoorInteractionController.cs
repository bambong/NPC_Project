using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityDoorInteractionController : MonoBehaviour, IInteraction
{
    [SerializeField]
    private GameObject sphere;

    private SphereCollider sphereTrigger;
    private BoxCollider boxCollider;

    private bool isInteractable = true;

    public GameObject Go => gameObject;
    public bool IsInteractAble => isInteractable;

    public void InteractionOn() => isInteractable = true;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        sphereTrigger = sphere.GetComponent<SphereCollider>();
        sphereTrigger.enabled = false;
    }

    public void OnInteraction()
    {
        //캡스 창 켜기
        isInteractable = false;
        Managers.Game.Player.SetStateIdle();
        boxCollider.enabled = false;
        sphereTrigger.enabled = true;
    }    
}
