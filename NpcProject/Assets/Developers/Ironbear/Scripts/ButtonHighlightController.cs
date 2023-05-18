using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightController : UI_Base, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image highlightImage;

    private Tweener highlightTween;

    private float highlightDuration = 0.2f;
    private Color transparentColor = new Color(1f, 1f, 1f, 0f);
    private Color highlightColor = new Color(1f, 1f, 1f, 1f);

    public override void Init()
    {
    }

    private void Awake()
    {
        if (highlightImage == null)
        {
            highlightImage = GetComponent<Image>();
        }
        highlightImage.color = transparentColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightTween != null && highlightTween.IsPlaying())
        {
            highlightTween.Kill();
        }

        highlightTween = highlightImage.DOColor(transparentColor, highlightDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightTween != null && highlightTween.IsPlaying())
        {
            highlightTween.Kill();
        }

        highlightTween = highlightImage.DOColor(highlightColor, highlightDuration).SetEase(Ease.OutQuad);
    }
}
