using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamePanelController : MonoBehaviour
{
    [SerializeField]
    private Image header;
    [SerializeField]
    private Image body;

    [SerializeField]
    private float headerOpenTime = 1f;
    [SerializeField]
    private float bodyOpenTime = 1f;
    [SerializeField]
    private Vector2 headerDesireSize;
    [SerializeField]
    private Vector2 bodyDesireSize;

    [SerializeField]
    private float titleLogoFadeTime = 0.2f;
    [SerializeField]
    private float textTitleOpenTime = 0.5f;

    [SerializeField]
    private Image titleLogo;
    [SerializeField]
    private TextMeshProUGUI textTitle;

    [SerializeField]
    private string text;

    private Sequence openSeq;
    private Sequence CloseSeq;
    private Vector2 bodyMinmumSize;
    private Vector2 headMinmumSize;
    private bool isOpen = false;
    public void Open(float openTime, Action action = null) 
    {
        if(isOpen)
        {
            return;
        }
        isOpen = true;
        if (CloseSeq != null && CloseSeq.IsPlaying())
        {
            CloseSeq.Kill();
            CloseSeq = null;
        }
        //if(openSeq != null && openSeq.IsPlaying())
        //{
        //    openSeq.Kill();
        //    openSeq = null;
        //}
        header.color = Color.black;
        body.color = Color.black;
        headMinmumSize = new Vector2(0, headerDesireSize.y);
        bodyMinmumSize = new Vector2(bodyDesireSize.x, 0);
        header.rectTransform.sizeDelta = headMinmumSize;
        body.rectTransform.sizeDelta = bodyMinmumSize;
        titleLogo.color = new Color(1, 1, 1, 0);
        textTitle.text = "";
        openSeq = DOTween.Sequence();
        openSeq.AppendInterval(openTime);
        openSeq.Append(header.rectTransform.DOSizeDelta(headerDesireSize, headerOpenTime));
        openSeq.Join(header.DOColor(Color.white ,headerOpenTime/3).SetLoops(3));
        openSeq.Append(body.rectTransform.DOSizeDelta(bodyDesireSize,bodyOpenTime));
        openSeq.Join(body.DOColor(Color.white, bodyOpenTime / 3).SetLoops(3));
        openSeq.Join(textTitle.DOText(text, textTitleOpenTime));
        openSeq.Join(titleLogo.DOFade(1, titleLogoFadeTime));
        openSeq.OnComplete(() => { action?.Invoke(); openSeq = null; });
        openSeq.Play();
    }
    public void Close(Action action = null) 
    {
        if(!isOpen)
        {
            return;
        }
        isOpen = false;
        //if(CloseSeq != null && CloseSeq.IsPlaying())
        //{
        //    CloseSeq.Kill();
        //    CloseSeq = null;
        //}
        if(openSeq != null && openSeq.IsPlaying())
        {
            openSeq.Kill();
            openSeq = null;
        }

        textTitle.text = "";
        titleLogo.color = new Color(1, 1, 1, 0);
        CloseSeq = DOTween.Sequence();
        CloseSeq.Append(body.rectTransform.DOSizeDelta(bodyMinmumSize, bodyOpenTime));
        CloseSeq.Append(header.rectTransform.DOSizeDelta(headMinmumSize, headerOpenTime));
        CloseSeq.OnComplete(() => { action?.Invoke(); CloseSeq = null; });
        CloseSeq.Play();
    }

}
