using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class ResultButtonController : MonoBehaviour ,IPointerEnterHandler ,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField]
    private CanvasGroup focusImageCanvas;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private Color focusColor;

    [SerializeField]
    private float blinkTime = 0.3f;
    [SerializeField]
    private float openTextTimt = 0.5f;
    [SerializeField]
    private float closeTextTimt = 0.3f;

    [SerializeField]
    private float focusOpenTime = 0.3f;
    [SerializeField]
    private float focusCloseTime = 0.2f;

    private Action onClick;
    private bool iSAvailable = false;
    private void Start()
    {
        Clear();
    }
    public void Open(float interval , string text , Action click , Action onComplete = null)
    {
        if (iSAvailable) 
        {
            return;
        }
        Clear();
        onClick = click;
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(interval);
        seq.Append(titleText.DOText(text, openTextTimt));
        seq.OnComplete(() => { iSAvailable = true; onComplete?.Invoke(); });
        seq.Play();
    }
    public void Close(float interval , Action onComplete = null) 
    {
        iSAvailable = false;
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(interval);
        seq.Append(titleText.DOText("", closeTextTimt));
        seq.Join(focusImageCanvas.DOFade(0,closeTextTimt));
        seq.OnComplete(() => { onComplete?.Invoke(); });
        seq.Play();
    }
    public void Clear() 
    {
        titleText.color = Color.white;
        iSAvailable = false;
        titleText.text = "";
        focusImageCanvas.alpha = 0;
        onClick = null;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!iSAvailable) 
        {
            return; 
        }
        onClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!iSAvailable)
        {
            return;
        }

        focusImageCanvas.DOKill();
        float curTime = focusOpenTime * (1 - focusImageCanvas.alpha);
        focusImageCanvas.DOFade(1, curTime);
        titleText.color = focusColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!iSAvailable)
        {
            return;
        }
        focusImageCanvas.DOKill();
        float curTime = focusCloseTime * focusImageCanvas.alpha;
        focusImageCanvas.DOFade(0, curTime);
        titleText.color = Color.white;
    }

}
