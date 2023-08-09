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
    private GameObject mouse;
    [SerializeField]
    private CanvasGroup keywordsCanvasgroup;

    private PanelsController panelsController;

    private int currentFrameIndex = 0;

    private void Start()
    {
        panelsController = GetComponentInParent<PanelsController>();
        
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

        Sequence mouseBlink = DOTween.Sequence();
        mouseBlink.Append(mouse.GetComponent<CanvasGroup>().DOFade(0f, 1f).SetEase(Ease.OutQuad));
        mouseBlink.Append(mouse.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetEase(Ease.OutQuad));
        mouseBlink.SetLoops(-1);
        mouseBlink.Play();
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
            //else if (currentFrameIndex == frames.Length - 1)
            //{
            //    //마지막 퍼즐이 완성되면 다음 패널 로딩
            //    keywordsCanvasgroup.blocksRaycasts = false;
            //    keywordsCanvasgroup.alpha = 0f;
            //    panelsController.CanvasFadeOut();
            //}
        });

        if (currentFrameIndex == frames.Length - 1)
        {
            //마지막 퍼즐이 완성되면 다음 패널 로딩
            keywordsCanvasgroup.blocksRaycasts = false;
            keywordsCanvasgroup.alpha = 0f;
            panelsController.CanvasFadeOut();
        }


        //if (currentFrameIndex < frames.Length - 1)
        //{
        //    IntroKeywordFrameController nextFrame = frames[currentFrameIndex + 1];
        //    nextFrame.FadeIn();

        //    currentFrameIndex++;
        //    isFilled = false;
        //}
        //else if (currentFrameIndex == frames.Length - 1)
        //{
        //    // 마지막 퍼즐이 완성되면 다음 패널 로딩
        //    keywordsCanvasgroup.blocksRaycasts = false;
        //    keywordsCanvasgroup.alpha = 0f;
        //    panelsController.CanvasFadeOut();
        //}
    }

    public void SetFilled()
    {
        isFilled = true;
    }
}
