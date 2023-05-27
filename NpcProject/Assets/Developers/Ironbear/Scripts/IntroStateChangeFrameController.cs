using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class IntroStateChangeFrameController : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject statePanel;
    [SerializeField]
    private GameObject door;

    private CanvasGroup stateCanvas;

    private float fadeDuration = 0.5f;

    private void Start()
    {
        stateCanvas = statePanel.GetComponent<CanvasGroup>();
        door.transform.localPosition = new Vector3(1f, 0f, -68.5f);
        door.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);

            RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
            draggedRect.localScale = Vector3.one;
            draggedRect.DOLocalMove(Vector3.zero, 0.1f).SetEase(Ease.OutQuad);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                stateCanvas.DOFade(0f, fadeDuration);
                //문 열림...
                door.transform.localPosition = Vector3.zero;
                door.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            });
            
        }
    }
}
