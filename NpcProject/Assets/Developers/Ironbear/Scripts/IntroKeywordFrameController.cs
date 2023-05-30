using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class IntroKeywordFrameController : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private CanvasGroup canvasGroup;
    private PuzzlePanelController panelController;

    private float initScale = 1.3f;
    private float targetScale = 1.5f;
    private float animDuration = 0.1f;
    private float fadeDuration = 0.5f;

    private void Awake()
    {
        image = GetComponent<Image>();
        canvasGroup = transform.parent.GetComponentInParent<CanvasGroup>();
        panelController = transform.parent.GetComponentInParent<PuzzlePanelController>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && !panelController.isFilled)
        {
            Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleGood);
            panelController.SetFilled();
            eventData.pointerDrag.transform.SetParent(transform);

            RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
            CanvasGroup draggedCanvasGroup = eventData.pointerDrag.GetComponent<CanvasGroup>();

            draggedCanvasGroup.blocksRaycasts = false;
            draggedRect.localScale = Vector3.one;
            draggedRect.DOLocalMove(Vector3.zero, 0.1f).SetEase(Ease.OutQuad);
            
            panelController.FadeOutCurrentFrame();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.rectTransform.DOScale(Vector3.one * targetScale, animDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!panelController.isFilled)
        {
            image.rectTransform.DOScale(Vector3.one * initScale, animDuration).SetEase(Ease.OutBack);
        }        
    }

    public void SetAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }

    public void FadeOut()
    {
        canvasGroup.DOFade(0f, fadeDuration * 1.3f);
    }

    public void FadeIn()
    {
        canvasGroup.DOFade(1f, fadeDuration);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
