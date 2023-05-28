using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TextPanelController : UI_Base
{
    [SerializeField]
    private string[] texts;
    [SerializeField]
    private TMP_Text tmpText;

    private float fadeDuration = 1f;
    private float typeSpeed = 0.08f;

    private Sequence seq;
    private CanvasGroup canvas;



    public override void Init()
    {

    }

    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        seq = DOTween.Sequence();
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
                    seq.AppendInterval(0.8f);
                    tmpText.text = " ";
                });
            }
            seq.AppendInterval(0.5f);
        }

        seq.OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.5f, () =>
             {
                 canvas.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad).OnComplete(() =>
                 {
                     SceneManager.LoadScene("Chapter_01_Office_Slave");
                 });
                 
             });           
        });

        seq.Play();
    }


}
