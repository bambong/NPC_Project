using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ResultNodeController : UI_Base
{

    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Image outImage;
    [SerializeField]
    private Image outlineImage;


    [SerializeField]
    private RectTransform rectTransform;
    private string answerKey;
    private bool isSuccess = false;
    public string AnswerKey { get => answerKey;}
    public bool IsSuccess { get => isSuccess; }

    public void ClearSize() 
    {
        rectTransform.localScale = Vector2.zero;
        rectTransform.localPosition = Vector3.zero;
    }

    public void SetKey(string key) 
    {
        answerKey = key;
        SetTextUI(key);
    }
    public void SetTextUI(string txt) 
    {
        text.text = txt;
    }
    public void SetIsSuccess(bool isOn)
    {
        isSuccess = isOn;
        if (isOn)
        {
            outImage.color = Color.green;
        }
        else
        {
            outImage.color = Color.red;
        }
    }
    public void OpneAnim(float interval)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(interval);
        sequence.Append(rectTransform.DOScale(1, 0.2f));
        sequence.Play();
    }
    public void CloseAnim(float interval)
    { 
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(interval);
        sequence.Append(rectTransform.DOScale(0, 0.2f));
        sequence.Play();
    }
    public void ResetNode()
    {
        //SetKey("");
        isSuccess = false;
        outlineImage.DOKill();
        outlineImage.DOFade(0, 0);
        outImage.color = Color.white;
    }

    public void SetOutline(bool isOn) 
    {
        outlineImage.DOKill();
        if (isOn) 
        {
            outlineImage.gameObject.SetActive(true);
            outlineImage.DOFade(0, 0);
            outlineImage.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        else 
        {
            outlineImage.gameObject.SetActive(false);
        }
    }

    public override void Init()
    {
  
    }
}
