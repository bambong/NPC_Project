using UnityEngine;

public class MySeatController : MonoBehaviour, IInteraction
{
    [SerializeField]
    private GameObject statePanel;

    private bool isInteractable = false;

    public GameObject Go => gameObject;
    public bool IsInteractAble => isInteractable;

    public void InteractionOn() => isInteractable = true;

    public void OnInteraction()
    {
        statePanel.SetActive(true);
        isInteractable = false;
    }
}
