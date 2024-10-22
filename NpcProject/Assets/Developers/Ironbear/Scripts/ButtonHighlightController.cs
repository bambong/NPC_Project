using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightController : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image highlightImage;
    [SerializeField]
    private Color highlightColor = new Color(1f, 1f, 1f, 1f); //이거 수정하면 된당!

    private Tweener highlightTween;
    private float highlightDuration = 0.2f;
    private Color transparentColor = new Color(1f, 1f, 1f, 0f);
    private bool btnSoundPlay = true;

    public override void Init()
    {
    }

    private void Awake()
    {
        if (highlightImage == null)
        {
            highlightImage = GetComponent<Image>();
        }
        ClearHighlightImage();
    }
    public void ClearHighlightImage() 
    {
        highlightImage.color = transparentColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightTween != null)
        {
            highlightTween.Kill();
        }
        highlightTween = highlightImage.DOColor(transparentColor, highlightDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightTween != null)
        {
            highlightTween.Kill();
        }
        if(btnSoundPlay)
        {
            Managers.Sound.PlaySFX(Define.SOUND.ButtonHover);
        }        
        highlightTween = highlightImage.DOColor(highlightColor, highlightDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        btnSoundPlay = false;
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleGood);
    }

}
