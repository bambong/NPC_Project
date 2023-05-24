using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class IntroKeywordFrameController : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject nextText;

    private Image image;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private CanvasGroup nextCanvasGroup;

    private bool isFilled = false;

    private float initScale = 1.3f;
    private float targetScale = 1.5f;
    private float animDuration = 0.1f;
    private float fadeOutDuration = 0.5f;
    private float fadeInDuration = 0.5f;

    private Sequence seq;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        canvasGroup = GetComponentInParent<CanvasGroup>();
        nextCanvasGroup = nextText.GetComponent<CanvasGroup>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && isFilled == false)
        {
            isFilled = true;
            eventData.pointerDrag.transform.SetParent(transform);

            RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
            draggedRect.localScale = Vector3.one;
            draggedRect.DOLocalMove(Vector3.zero, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                FadeOutFrame();
                seq.AppendInterval(1f);
                FadeInFrame();
            });
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.rectTransform.DOScale(Vector3.one * targetScale, animDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isFilled)
        {
            image.rectTransform.DOScale(Vector3.one * initScale, animDuration).SetEase(Ease.OutBack);
        }        
    }

    private void FadeOutFrame()
    {
        canvasGroup.DOFade(0f, fadeOutDuration);
    }

    private void FadeInFrame()
    {
        nextText.SetActive(true);
        nextCanvasGroup.alpha = 0f;
        nextCanvasGroup.DOFade(1f, fadeInDuration);
    }
}
