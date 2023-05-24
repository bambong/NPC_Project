using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelsController : UI_Base
{
    [SerializeField]
    private CanvasGroup canvas;
    [SerializeField]
    private GameObject logoPanel;
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject puzzlePanel;

    private float fadeDuration = 1f;

    private bool isLogo = false;
    private bool isStart = false;
    private bool isPuzzle = false;


    public override void Init()
    {
    }

    private void Awake()
    {
        logoPanel.SetActive(true);
        startPanel.SetActive(false);
        puzzlePanel.SetActive(false);
        isLogo = true;
    }

    public void CanvasFadeOut()
    {
        canvas.alpha = 1f;
        canvas.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad).OnComplete(() => {
            if(isLogo)
            {
                logoPanel.SetActive(false);
                startPanel.SetActive(true);
                isLogo = false;
                isStart = true;
            }
            else if(isStart)
            {
                startPanel.SetActive(false);
                puzzlePanel.SetActive(true);
                isStart = false;
                isPuzzle = true;
            }
            else if(isPuzzle)
            {
                //scene loading
            }
            
            CanvasFadeIn(); });
    }

    private void CanvasFadeIn()
    {
        canvas.alpha = 0f;
        canvas.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);
    }
}
