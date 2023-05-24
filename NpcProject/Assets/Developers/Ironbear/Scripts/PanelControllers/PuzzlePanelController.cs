using UnityEngine;
using DG.Tweening;

public class PuzzlePanelController : MonoBehaviour
{
    [SerializeField]
    private IntroKeywordFrameController[] frames;
    [SerializeField]
    private CanvasGroup[] groups;

    public bool isFilled = false;
    private int currentFrameIndex = 0;

    private void Awake()
    {
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
        });       
    }

    public void SetFilled()
    {
        isFilled = true;
    }

    /*
    [SerializeField]
    private GameObject txt1;
    [SerializeField]
    private GameObject txt2;
    [SerializeField]
    private GameObject txt3;
    [SerializeField]
    private GameObject txt4;

    private IntroKeywordFrameController txt1Controller;
    private IntroKeywordFrameController txt2Controller;
    private IntroKeywordFrameController txt3Controller;
    private IntroKeywordFrameController txt4Controller;

    private CanvasGroup canvas1;
    private CanvasGroup canvas2;
    private CanvasGroup canvas3;
    private CanvasGroup canvas4;

    private bool isTxt1Filled;
    private bool isTxt2Filled;
    private bool isTxt3Filled;
    private bool isTxt4Filled;

    private float fadeOutDuration = 0.5f;
    private float fadeInDuration = 0.5f;

    private void Awake()
    {
        canvas1 = txt1.GetComponentInParent<CanvasGroup>();
        canvas2 = txt2.GetComponentInParent<CanvasGroup>();
        canvas3 = txt3.GetComponentInParent<CanvasGroup>();
        canvas4 = txt4.GetComponentInParent<CanvasGroup>();

        canvas1.alpha = 1f;
        canvas2.alpha = 0f;
        canvas3.alpha = 0f;
        canvas4.alpha = 0f;

        txt1Controller = txt1.GetComponent<IntroKeywordFrameController>();
        txt2Controller = txt2.GetComponent<IntroKeywordFrameController>();
        txt3Controller = txt3.GetComponent<IntroKeywordFrameController>();
        txt4Controller = txt4.GetComponent<IntroKeywordFrameController>();
    }

    private void Update()
    {
        isTxt1Filled = txt1Controller.IsFilled();
        isTxt2Filled = txt2Controller.IsFilled();
        isTxt3Filled = txt3Controller.IsFilled();
        isTxt4Filled = txt4Controller.IsFilled();

        if(isTxt1Filled)
        {
            DOVirtual.DelayedCall(0.8f, () =>
            {
                canvas1.DOFade(0f, fadeOutDuration).OnComplete(() =>
                {
                    canvas2.DOFade(1f, fadeInDuration);
                });
            });                     
        }

        if(isTxt2Filled)
        {
            DOVirtual.DelayedCall(0.8f, () =>
            {
                canvas2.DOFade(0f, fadeOutDuration).OnComplete(() =>
                {
                    canvas3.DOFade(1f, fadeInDuration);
                });
            });
        }

        if(isTxt3Filled)
        {
            DOVirtual.DelayedCall(0.8f, () =>
            {
                canvas3.DOFade(0f, fadeOutDuration).OnComplete(() =>
                {
                    canvas4.DOFade(1f, fadeInDuration);
                });
            });
        }

        if(isTxt4Filled)
        {
            //complete animation
        }
    }
    */
}
