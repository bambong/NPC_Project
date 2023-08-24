using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using FMODUnity;

public class LogoPanelController : UI_Base
{
    [SerializeField]
    private EventReference bgm;
    [SerializeField]
    private GameObject fakeBgm;

    [SerializeField]
    private GameObject logos;
    [SerializeField]
    private GameObject btns;
    [SerializeField]
    private GameObject title;

    [SerializeField]
    private GameObject[] btnsList;

    [SerializeField]
    private Material effectMat;
    [SerializeField]
    private GameObject logo;

    [SerializeField]
    private GameObject firstPanel;
    [SerializeField]
    private CanvasGroup secondPanel;

    private Image rend;

    [SerializeField]
    private CanvasGroup firstPanelCanvas;
    [SerializeField]
    private CanvasGroup txtCanvasGroup;

    private float fadeInDuration = 1f;
    private float fadeOutDuration = 1f;
    private float blinkInterval = 1f;

    private bool isEndEffect = false;
    private bool isOpenPanel = false;

    public override void Init()
    {
        for (int i = 0; i < btnsList.Length; i++)
        {
            btnsList[i].GetComponent<Button>().interactable = false;
        }
    }

    private void Start()
    {
        rend = logo.GetComponent<Image>();
        rend.material = effectMat;
        rend.material.SetFloat("_AutoManualAnimation", 0);
        firstPanel.SetActive(true);
        secondPanel.interactable = false;
        secondPanel.blocksRaycasts = false;
        secondPanel.alpha = 0;

        //secondPanel.SetActive(false);
        //btns.GetComponent<CanvasGroup>().alpha = 0f;

        Blink();   
    }

    private void Update()
    {
        if(Input.anyKeyDown && isEndEffect)
        {
            FadePanelEffect();
        }
    }

    private void FadePanelEffect()
    {
        if (isOpenPanel) 
        {
            return;
        }
        isOpenPanel = true;

        Managers.Sound.StopBGM();
        fakeBgm.SetActive(true);
        //Managers.Sound.ChangeBGM(bgm);
        //Managers.Sound.PlayBGM();
        secondPanel.interactable = true;
        secondPanel.blocksRaycasts = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(firstPanelCanvas.DOFade(0f, fadeInDuration)
            .SetEase(Ease.Linear));
       //seq.Append(.DOFade(1f, fadeInDuration * 2.5f).SetEase(Ease.Linear));
        seq.Append(secondPanel.DOFade(1f, fadeInDuration * 2.5f).SetEase(Ease.Linear));
        seq.OnComplete(() =>
            {
                firstPanel.SetActive(false);
               
                //secondPanel.SetActive(true);
                //btns.GetComponent<CanvasGroup>().DOFade(1f, fadeInDuration * 2.5f).SetEase(Ease.Linear);
            });
        seq.Play();
    }

    private void Blink()
    {
        txtCanvasGroup.DOFade(0f, fadeInDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                txtCanvasGroup.DOFade(1f, fadeOutDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        DOVirtual.DelayedCall(blinkInterval, Blink);
                    });
            });

        Invoke("RestoreMaterial", 1.6f);
    }

    private void RestoreMaterial()
    {
        rend.material.SetFloat("_AutoManualAnimation", 1);
        isEndEffect = true;
    }

    public void UninteractiveButtons()
    {
        btns.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void InteractiveButtons()
    {
        btns.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        //btnsList[2].gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
