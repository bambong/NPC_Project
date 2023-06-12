using UnityEngine;
using DG.Tweening;

public class PuzzlePanelController : MonoBehaviour
{
    [HideInInspector]
    public bool isFilled = false;

    [SerializeField]
    private IntroKeywordFrameController[] frames;
    [SerializeField]
    private CanvasGroup[] groups;

    [SerializeField]
    private GameObject mouseImage;

    private PanelsController panelsController;
    private CanvasGroup mouseCanvas;

    private int currentFrameIndex = 0;

    private void Start()
    {
        panelsController = GetComponentInParent<PanelsController>();
        mouseCanvas = mouseImage.GetComponent<CanvasGroup>();
        
        for (int i = 0; i < groups.Length; i++)
        {
            if (i == 0)
            {
                groups[i].alpha = 1f;
                groups[i].interactable = true;
                groups[i].blocksRaycasts = true;
            }
            else
            {
                groups[i].alpha = 0f;
                groups[i].interactable = false;
                groups[i].blocksRaycasts = false;
            }
        }

        Sequence mouseAlphaSeq = DOTween.Sequence();
        mouseAlphaSeq.Append(mouseCanvas.DOFade(0f, 1.5f).SetEase(Ease.OutQuad));
        mouseAlphaSeq.Append(mouseCanvas.DOFade(1f, 1.5f).SetEase(Ease.OutQuad));
        mouseAlphaSeq.SetLoops(-1);
        mouseAlphaSeq.Play();
    }

    public void FadeOutCurrentFrame()
    {
        IntroKeywordFrameController currentFrame = frames[currentFrameIndex];
        currentFrame.FadeOut();

        DOVirtual.DelayedCall(0.8f, () =>
        {
            if (currentFrameIndex < frames.Length - 1)
            {
                IntroKeywordFrameController nextFrame = frames[currentFrameIndex + 1];
                nextFrame.FadeIn();

                currentFrameIndex++;
                isFilled = false;
            }
            else if (currentFrameIndex == frames.Length - 1)
            {
                //마지막 퍼즐이 완성되면 다음 패널 로딩
                panelsController.CanvasFadeOut();
            }
        });       
    }

    public void SetFilled()
    {
        isFilled = true;
    }
}
