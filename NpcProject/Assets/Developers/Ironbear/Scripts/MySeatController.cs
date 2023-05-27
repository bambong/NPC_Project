using UnityEngine;

public class MySeatController : MonoBehaviour, IInteraction
{
    [SerializeField]
    private GameObject statePanel;

    private bool isInteractable = true;

    public GameObject Go => gameObject;
    public bool IsInteractAble => isInteractable;

    public void OnInteraction()
    {
        statePanel.SetActive(true);
        isInteractable = false;
    }
}
