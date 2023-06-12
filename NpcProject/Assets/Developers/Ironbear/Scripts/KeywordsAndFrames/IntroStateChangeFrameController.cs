using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;

public class IntroStateChangeFrameController : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject statePanel;
    [SerializeField]
    private GameObject myseat;
   

    private CanvasGroup parentCanvas;
    private CanvasGroup stateCanvas;
    private StatePanelController statePanelController;

    private float fadeDuration = 0.5f;

    private void Start()
    {
        parentCanvas = GetComponentInParent<CanvasGroup>();
        stateCanvas = statePanel.GetComponent<CanvasGroup>();
        statePanelController = statePanel.GetComponent<StatePanelController>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);

            RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
            draggedRect.localScale = Vector3.one;
            draggedRect.DOLocalMove(Vector3.zero, 0.1f).SetEase(Ease.OutQuad);

            parentCanvas.interactable = false;
            parentCanvas.blocksRaycasts = false;
            myseat.SetActive(false);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                stateCanvas.DOFade(0f, fadeDuration).OnComplete(() =>
                {
                    statePanel.SetActive(false);
                });

                //문 열림...
                Managers.Game.Player.SetStateIdle();
                statePanelController.DoorOpen();
                statePanelController.FloorArrowEffect();
            });
            
        }
    }
}
