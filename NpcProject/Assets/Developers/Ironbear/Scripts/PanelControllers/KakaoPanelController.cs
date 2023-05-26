using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

    void Start()
    {
        yesTxt.SetActive(false);
        yupTxt.SetActive(false);

        for(int i=0; i<kakaos.Length; i++)
        {
            kakaos[i].SetActive(false);
        }

        OpenKakaoTalk();
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
        StartCoroutine(Waiting());
        CloseKakaoTalk();
    }

    public void Yup()
    {
        yupTxt.SetActive(true);
        StartCoroutine(Waiting());
        CloseKakaoTalk();
    }

    private void OpenKakaoTalk()
    {
        for (int i = 0; i < kakaos.Length; i++)
        {
            kakaos[i].SetActive(true);
            StartCoroutine(Waiting());
        }
    }

    private void CloseKakaoTalk()
    {
        for(int i=0; i<kakaos.Length; i++)
        {
            kakaos[i].SetActive(false);
        }
        yesTxt.SetActive(false);
        yupTxt.SetActive(false);
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(2f);
    }
}
