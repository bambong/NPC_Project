using UnityEngine;
using DG.Tweening;

public class LetterBoxPanelController : UI_Base
{
    [SerializeField]
    private GameObject upBox;
    [SerializeField]
    private GameObject downBox;

    private RectTransform upRect;
    private RectTransform downRect;

    private float animDuration = 3f;
    private float letterboxAmount = 0.1f;
    private bool isLetterboxIn;
    private bool isAnimating = false;
    private float letterboxRatio = 1.2f;

    private void Awake()
    {
        upRect = upBox.GetComponent<RectTransform>();
        downRect = downBox.GetComponent<RectTransform>();
    }

    public override void Init()
    {
        InitializeLetterBox();
    }

    private void InitializeLetterBox()
    {
        float targetHeight = Screen.width * letterboxAmount;

        upRect.sizeDelta = new Vector2(Screen.width, upRect.sizeDelta.y);
        downRect.sizeDelta = new Vector2(Screen.width, downRect.sizeDelta.y);

        upRect.offsetMin = new Vector2(upRect.offsetMin.x, Screen.height/2);
        upRect.offsetMax = new Vector2(upRect.offsetMax.x, Screen.height / 2 + targetHeight);

        downRect.offsetMin = new Vector2(downRect.offsetMin.x, -Screen.height / 2 - targetHeight);
        downRect.offsetMax = new Vector2(downRect.offsetMax.x, -Screen.height / 2);
    }

    public void AnimationLetterBoxIn()
    {
        if(isAnimating || isLetterboxIn)
        {
            return;
        }

        float targetHeight = Screen.width * letterboxAmount;
        //isAnimating = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(upRect.DOLocalMoveY(upRect.offsetMax.y - targetHeight * letterboxRatio, animDuration).SetEase(Ease.InOutQuad));
        seq.Join(downRect.DOLocalMoveY(downRect.offsetMin.y + targetHeight * letterboxRatio, animDuration).SetEase(Ease.InOutQuad));
        seq.OnComplete(() =>
        {
            isAnimating = false;
            isLetterboxIn = true;
        });

        seq.Play();
        isAnimating = true;
    }

    public void AnimationLetterBoxOut()
    {
        if(isAnimating || !isLetterboxIn)
        {
            return;
        }

        float targetHeight = Screen.width * letterboxAmount;
        //isAnimating = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(upRect.DOLocalMoveY(upRect.offsetMax.y + targetHeight * letterboxRatio, animDuration).SetEase(Ease.InOutQuad));
        seq.Join(downRect.DOLocalMoveY(downRect.offsetMin.y - targetHeight * letterboxRatio, animDuration).SetEase(Ease.InOutQuad));
        seq.OnComplete(() =>
        {
            isAnimating = false;
            isLetterboxIn = false;
        });
        seq.Play();
        isAnimating = true;
    }
}
