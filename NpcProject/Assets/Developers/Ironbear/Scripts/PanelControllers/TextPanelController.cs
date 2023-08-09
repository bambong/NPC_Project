using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class TextPanelController : UI_Base
{
    [TextArea]
    [SerializeField]
    private string[] texts;
    [SerializeField]
    private TMP_Text tmpText;
    [SerializeField]
    private Define.Scene transitionSceneName;
    [SerializeField]
    private TMP_Text continueTxt;

    private float fadeDuration = 1f;
    private float typeSpeed = 0.04f;    

    private Sequence seq;
    private Sequence txtSeq;
    private CanvasGroup canvas;
    private CanvasGroup txtCanvas;

    private float soundSpeed = 0.1f;
    private bool textplayed;
    private bool isTextComplete = false;

    public override void Init()
    {

    }

    private void Start()
    {
        txtCanvas = continueTxt.gameObject.GetComponent<CanvasGroup>();
        canvas = GetComponent<CanvasGroup>();
        seq = DOTween.Sequence();
        txtSeq = DOTween.Sequence();

        continueTxt.gameObject.SetActive(false);
        txtCanvas.alpha = 0f;
        TypeAnimation();
    }

    private void TypeAnimation()
    {
        for (int j = 0; j < texts.Length; j++)
        {
            string text = texts[j];
            text = text.Replace("\\n", "\n");

            seq.Append(tmpText.DOText(text, text.Length * typeSpeed).SetEase(Ease.Linear)
                .OnStart(() => textplayed = false)
                .OnStart(() => TextSound(text.Length * soundSpeed))
                .OnComplete(()=> textplayed = true));
            

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
            //isTextComplete = true;
            continueTxt.gameObject.SetActive(true);

            txtSeq.Append(continueTxt.gameObject.transform.DOLocalMoveY(continueTxt.gameObject.transform.position.y - 70f, 1f));
            txtSeq.Join(txtCanvas.DOFade(1f, 1f));
            txtSeq.Play();

            isTextComplete = true;
        });

        seq.Play();
    }

    private void Update()
    {
        if(isTextComplete && Input.anyKeyDown)
        {
            isTextComplete = false;
            canvas.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Managers.Scene.LoadScene(transitionSceneName.ToString());
            });
        }
    }

    public void TextSound(float time)
    {
        StartCoroutine(PlayTextSound(time));
    }

    IEnumerator PlayTextSound(float time)
    {
        float texttime = 0;

        while (time > texttime && !textplayed)
        {
            texttime += soundSpeed;
            Managers.Sound.PlaySFX(Define.SOUND.StartTextSound);
            yield return new WaitForSeconds(soundSpeed);
        }
    }

}
