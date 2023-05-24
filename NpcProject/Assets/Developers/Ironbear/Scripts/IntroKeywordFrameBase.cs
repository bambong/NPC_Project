using UnityEngine;
using DG.Tweening;

public class IntroKeywordFrameBase : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeOutDuration = 0.5f;
    public float fadeInDuration = 0.5f;

    private bool isFilled = false;

    public void SetFilledState(bool filled)
    {
        isFilled = filled;

        if(isFilled)
        {
            FadeOutUi();
        }
        else
        {
            FadeInUi();
        }
    }

    private void FadeOutUi()
    {
        canvasGroup.DOFade(0f, fadeOutDuration).OnComplete(() =>
        {
            //페이드아웃 완료 후 작업 수행
        });
    }

    private void FadeInUi()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, fadeInDuration).OnComplete(() =>
        {
            // 페이드인 완료 후에 수행할 작업
        });
    }
}
