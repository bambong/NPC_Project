using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;

public class SecurityCapController : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject sphere;
    [SerializeField]
    private GameObject capPanel;
    [SerializeField]
    private GameObject locked;
    [SerializeField]
    private GameObject unlocked;

    private CanvasGroup lockedCanvas;
    private CanvasGroup unlockedCanvas;
    private CanvasGroup parentCanvas;
    private SphereCollider sphereTrigger;

    private float fadeDuration = 0.5f;

    private void Start()
    {
        lockedCanvas = locked.GetComponent<CanvasGroup>();
        unlockedCanvas = unlocked.GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<CanvasGroup>();
        sphereTrigger = sphere.GetComponent<SphereCollider>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);

            RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
            draggedRect.DOLocalMove(Vector3.zero, 0.1f).SetEase(Ease.OutQuad);

            parentCanvas.interactable = false;
            parentCanvas.blocksRaycasts = false;

            CapUnlock();
        }
    }

    private void CapUnlock()
    {
        sphereTrigger.enabled = true;
        lockedCanvas.alpha = 0f;
        unlockedCanvas.alpha = 1f;

        DOVirtual.DelayedCall(1f, () =>
        {
            capPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.5f);
            Managers.Game.Player.SetStateIdle();
        });       
    }
}
