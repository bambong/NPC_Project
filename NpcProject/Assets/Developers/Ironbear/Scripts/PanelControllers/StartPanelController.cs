using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class StartPanelController : UI_Base
{
    [SerializeField]
    private string[] texts;
    [SerializeField]
    private TMP_Text tmpText;
    [SerializeField]
    private RectTransform btn;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private GameObject mouse;

    private float typeSpeed = 0.08f;
    private Sequence seq;

    public override void Init()
    {
        
    }

    private void Start()
    {
        seq = DOTween.Sequence();
        btn.GetComponent<Button>().interactable = false;
        canvasGroup.alpha = 0f;
        mouse.GetComponent<CanvasGroup>().alpha = 0f;
        btn.anchoredPosition = new Vector3(0f, -200, 0f);
        TypeAnimation();
    }

    private void TypeAnimation()
    {
        for (int j = 0; j < texts.Length; j++)
        {
            string text = texts[j];
            text = text.Replace("\\n", "\n");

            seq.Append(tmpText.DOText(text, text.Length * typeSpeed).SetEase(Ease.Linear));
            if (j < texts.Length - 1)
            {
                seq.AppendCallback(() =>
                {
                    seq.AppendInterval(0.5f);
                    tmpText.text = " ";
                });
            }
            seq.AppendInterval(0.1f);
        }

        seq.OnComplete(() => 
        {            
            btn.GetComponent<Button>().interactable = true;
            btn.DOAnchorPosY(280, 1f).SetEase(Ease.OutQuad);
            canvasGroup.DOFade(1f, 1f).SetEase(Ease.OutQuad);
            DOVirtual.DelayedCall(1.1f, ()=>
            {
                mouse.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    Sequence mouseSeq = DOTween.Sequence();
                    Vector3 originalPos = mouse.transform.position;
                    float mouseAnimDuration = 1f;

                    mouseSeq.Append(mouse.transform.DOMoveY(originalPos.y + 8f, mouseAnimDuration).SetEase(Ease.OutQuad));
                    mouseSeq.Join(mouse.GetComponent<CanvasGroup>().DOFade(0f, mouseAnimDuration).SetEase(Ease.OutQuad));
                //mouseSeq.SetLoops(-1);

                mouseSeq.Play();
                });
            });
            
        });

        seq.Play();
    }


}
