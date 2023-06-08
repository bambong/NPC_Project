using UnityEngine;

public class SecurityDoorInteractionController : MonoBehaviour, IInteraction
{
    [SerializeField]
    private GameObject sphere;
    [SerializeField]
    private GameObject capPanel;

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
        capPanel.SetActive(true);
        isInteractable = false;
        Managers.Game.Player.SetstateStop();
        boxCollider.enabled = false;
    }    
}
