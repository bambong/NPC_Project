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
    public override void Init()
    {
  
    }
}
