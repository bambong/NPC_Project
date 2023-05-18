using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ResultColorNodeController : UI_Base
{
    [SerializeField]
    private Image innerImage;
    [SerializeField]
    private Image outImage;

    [SerializeField]
    private RectTransform rectTransform;

    private int answerKey;
    private bool isSuccess = false;
    public int AnswerKey { get => answerKey;}
    public bool IsSuccess { get => isSuccess; }

    public void ClearSize() 
    {
        rectTransform.localScale = Vector3.zero;
        rectTransform.localPosition = Vector3.zero;
    }

    public void SetKey(int key , Color color) 
    {
        answerKey = key;
        SetInnerColor(color);
    }
    public void SetInnerColor(Color color) 
    {
        innerImage.DOColor(color,0.5f);
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
    public void OpenAnim(float interval)
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
        innerImage.color = new Color(0,0,0,0);
        isSuccess = false;
        outImage.color = Color.white;
    }
    public override void Init()
    {
        innerImage.color = new Color(0, 0, 0, 0);
    }
}
