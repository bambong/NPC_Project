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
    private GameObject btn;
    [SerializeField]
    private CanvasGroup canvasGroup;

    private float typeSpeed = 0.1f;
    private Sequence seq;
    


    public override void Init()
    {
        
    }

    private void Start()
    {
        seq = DOTween.Sequence();
        btn.GetComponent<Button>().interactable = false;
        canvasGroup.alpha = 0f;
        btn.transform.localPosition = new Vector3(0f, -200f, 0f);
        TypeAnimation();
    }

    private void TypeAnimation()
    {
        for (int j = 0; j < texts.Length; j++)
        {
            string text = texts[j];
            text = text.Replace("\\n", "\n");

            seq.Append(tmpText.DOText(text, text.Length * typeSpeed));
            if (j < texts.Length - 1)
            {
                seq.AppendCallback(() =>
                {
                    seq.AppendInterval(0.3f);
                    tmpText.text = " ";
                });
            }
            seq.AppendInterval(0.1f);
        }

        seq.OnComplete(() => 
        {
            btn.GetComponent<Button>().interactable = true;
            btn.transform.DOMoveY(450f, 1f).SetEase(Ease.OutQuad);
            canvasGroup.DOFade(1f, 1f).SetEase(Ease.OutQuad);
        });

        seq.Play();
    }


}
