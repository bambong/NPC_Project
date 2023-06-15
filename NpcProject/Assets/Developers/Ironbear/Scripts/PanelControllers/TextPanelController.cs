using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
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

    private float fadeDuration = 1f;
    private float typeSpeed = 0.04f;    

    private Sequence seq;
    private CanvasGroup canvas;

    private float soundSpeed = 0.1f;
    private bool textplayed;

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
            DOVirtual.DelayedCall(0.5f, () =>
             {
                 canvas.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad).OnComplete(() =>
                 {
                     Managers.Scene.LoadScene(transitionSceneName.ToString());
                 });
                 
             });           
        });

        seq.Play();

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
