using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LogoPanelController : UI_Base
{
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
    private GameObject secondPanel;

    private Image rend;

    //private float moveDuration = 0.8f;
    //private float moveDelay = 0.1f;

    [SerializeField]
    private CanvasGroup firstPanelCanvas;
    [SerializeField]
    private CanvasGroup txtCanvasGroup;
    private float fadeInDuration = 1f;

    private float fadeOutDuration = 1f;
    private float blinkInterval = 1f;


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
        secondPanel.SetActive(false);

        Blink();
        //logos.transform.localPosition = new Vector3(-1100, 0, 0);
        //for (int i = 0; i < btnsList.Length; i++)
        //{
        //    GameObject btn = btnsList[i];
        //    var pos = btn.transform.localPosition;
        //    pos.x -= 400;
        //    btn.transform.localPosition = pos; 
        //}
        //MoveObject();      
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            FadePanelEffect();
        }
    }

    private void FadePanelEffect()
    {
        firstPanelCanvas.DOFade(0f, fadeInDuration / 2)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                firstPanel.SetActive(false);
                secondPanel.SetActive(true);
            });
    }

    private void Blink()
    {
        txtCanvasGroup.DOFade(0f, fadeInDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                txtCanvasGroup.DOFade(1f, fadeOutDuration)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        DOVirtual.DelayedCall(blinkInterval, Blink);
                    });
            });

        Invoke("RestoreMaterial", 1.6f);
    }

    //private void MoveObject()
    //{
    //    logos.transform.DOLocalMoveX(logos.transform.localPosition.x + 1100, moveDuration).SetEase(Ease.OutQuad);

    //    for (int i = 0; i < btnsList.Length; i++)
    //    {
    //        GameObject btn = btnsList[i];
    //        float delay = i * moveDelay * 0.5f;

    //        btn.transform.DOLocalMoveX(btn.transform.localPosition.x + 400, moveDuration).SetEase(Ease.OutQuad).SetDelay(delay);
    //    }

    //    EnableButtonInteractable();
  
    //    Invoke("RestoreMaterial", moveDuration + 0.8f);
    //}

    private void RestoreMaterial()
    {
        rend.material.SetFloat("_AutoManualAnimation", 1);
    }

    private void EnableButtonInteractable()
    {
        for (int i = 0; i < btnsList.Length; i++)
        {
            btnsList[i].GetComponent<Button>().interactable = true;
        }
    }

    public void DisableButtonInteractable()
    {
        for (int i = 0; i < btnsList.Length; i++)
        {
            CanvasGroup btnCanvas = btnsList[i].GetComponent<CanvasGroup>();

            btnCanvas.blocksRaycasts = false;
            btnCanvas.interactable = false;
        }
    }
}
