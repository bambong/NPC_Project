using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class StartPanelController : UI_Base
{
    [SerializeField]
    [TextArea]
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
    private bool playedsound;

    public override void Init()
    {
        
    }

    private void Start()
    {
        seq = DOTween.Sequence();
        btn.GetComponent<Button>().interactable = false;
        btn.GetComponent<CanvasGroup>().blocksRaycasts = false;

        canvasGroup.alpha = 0f;
        mouse.GetComponent<CanvasGroup>().alpha = 0f;

        btn.anchoredPosition = new Vector3(0f, -200, 0f);
        TypeAnimation();
    }

    private void MouseEffect()
    {
        Sequence mouseSeq = DOTween.Sequence();
        CanvasGroup mouseCanvas = mouse.GetComponent<CanvasGroup>();

        mouseSeq.Append(mouseCanvas.DOFade(1f, 1.5f).SetEase(Ease.OutQuad));
        mouseSeq.Join(mouse.transform.DOLocalMoveY(mouse.transform.localPosition.y + 100f, 1.5f).SetEase(Ease.OutQuad));
        mouseSeq.AppendInterval(1.5f);
        mouseSeq.Append(mouseCanvas.DOFade(0f, 0.5f).SetEase(Ease.OutQuad));
        mouseSeq.Play();
    }

    private void TypeAnimation()
    {
        for (int j = 0; j < texts.Length; j++)
        {
            string text = texts[j];
            text = text.Replace("\\n", "\n");

            //corrections
            if (j != 1)
            {
                seq.Append(tmpText.DOText(text, text.Length * typeSpeed).SetEase(Ease.Linear)
                .OnStart(() => TextSound(text.Length * 0.08f)));
            }
            else
            {
                seq.Append(tmpText.DOText(text, text.Length * typeSpeed).SetEase(Ease.Linear));
            }


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
            btn.DOAnchorPosY(280, 1f).SetEase(Ease.OutQuad).OnComplete(() => 
            {
                btn.GetComponent<CanvasGroup>().blocksRaycasts = true;
                btn.GetComponent<Button>().interactable = true;
                MouseEffect(); 
            });

            canvasGroup.DOFade(1f, 1f).SetEase(Ease.OutQuad);                    
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

        while (time > texttime && !playedsound)
        {
            texttime += typeSpeed;
            Managers.Sound.PlaySFX(Define.SOUND.StartTextSound);
            yield return new WaitForSeconds(typeSpeed);
        }
    }


}
