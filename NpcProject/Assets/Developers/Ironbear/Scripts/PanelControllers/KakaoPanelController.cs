using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class KakaoPanelController : MonoBehaviour
{
    public Speaker player;

    [SerializeField]
    private GameObject[] kakaos;
    [SerializeField]
    private GameObject yesTxt;
    [SerializeField]
    private GameObject yupTxt;
    [SerializeField]
    private TMP_Text text1;
    [SerializeField]
    private GameObject[] btns;
    [SerializeField]
    private GameObject myseat;

    private float waitTime = 1.3f;


    void Start()
    {
        yesTxt.SetActive(false);
        yupTxt.SetActive(false);

        for (int i = 0; i < kakaos.Length; i++)
        {
            CanvasGroup kakaoCanvas = kakaos[i].GetComponent<CanvasGroup>();

            kakaoCanvas.alpha = 0f;
        }

        for (int i = 0; i < btns.Length; i++)
        {
            CanvasGroup btnCanvas = btns[i].GetComponent<CanvasGroup>();
            btns[i].SetActive(false);
        }

        OpenKakaoTalk();
    }

    private void OpenKakaoTalk()
    {
        Sequence openSq = DOTween.Sequence();
        float fadeDuration = 0.5f;
        float interval = 1f;

        for (int i = 0; i < kakaos.Length; i++)
        {
            CanvasGroup kakaoCanvas = kakaos[i].GetComponent<CanvasGroup>();

            openSq.Append(kakaoCanvas.DOFade(1f, fadeDuration));
            openSq.AppendInterval(interval);
        }

        openSq.Play().OnComplete(() =>
        {
            OpenButtons();
        });
    }

    private void OpenButtons()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            RectTransform btnRect = btns[i].GetComponent<RectTransform>();
            btns[i].SetActive(true);


            btnRect.localScale = new Vector3(0f, 1f, 1f);
            btnRect.pivot = new Vector2(0f, 0f);

            Sequence openSq = DOTween.Sequence();
            openSq.Append(btnRect.DOScaleX(1f, 0.3f).SetEase(Ease.OutSine));
            openSq.Play();
        }
    }

    private void CloseAll()
    {
        for(int i=0; i<kakaos.Length; i++)
        {
            CanvasGroup kakaoCanvas = kakaos[i].GetComponent<CanvasGroup>();
            kakaoCanvas.DOFade(0f, 0.45f).OnComplete(() =>
            {
                kakaos[i].SetActive(false);
            });
        }

        for(int i=0; i<btns.Length; i++)
        {
            CanvasGroup btnCanvas = btns[i].GetComponent<CanvasGroup>();
            btnCanvas.DOFade(0f, 0.45f).OnComplete(() =>
            {
                btns[i].SetActive(false);
            });
        }

        yesTxt.GetComponent<CanvasGroup>().DOFade(0f, 0.45f);
        yupTxt.GetComponent<CanvasGroup>().DOFade(0f, 0.45f);
    }

    public void UpdateName()
    {
        string dialogue = "<Player> 씨 오늘 할 일이 있으니 서버실로 와주세요.";
        dialogue = dialogue.Replace("<Player>", player.charName);
        text1.text = dialogue;
    }

    public void Yes()
    {
        yesTxt.SetActive(true);
        btns[1].GetComponent<Button>().interactable = false;

        DOVirtual.DelayedCall(waitTime, () =>
        {
            CloseAll();
            Managers.Game.Player.SetStateIdle();
            myseat.SetActive(true);
        });
    }

    public void Yup()
    {
        yupTxt.SetActive(true);
        btns[0].GetComponent<Button>().interactable = false;

        DOVirtual.DelayedCall(waitTime, () =>
        {
            CloseAll();
            Managers.Game.Player.SetStateIdle();
            myseat.SetActive(true);
        });
    }
}
