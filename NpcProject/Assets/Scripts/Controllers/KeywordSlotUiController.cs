using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordSlotUiController : UI_Base
{
    [SerializeField]
    private RectTransform keywordSlotLayout;
    [SerializeField]
    private RectTransform mask;
  
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

    [SerializeField]
    private RectTransform panelTrs;
    private readonly float OPEN_ANIM_TIME = 0.3f;
    private readonly float ClOSE_ANIM_TIME = 0.3f;
    public Transform KeywordSlotLayout { get => keywordSlotLayout.transform; }
 
    private bool isDrag = false;
    private bool isKeywordSlotOpen;
    private Sequence openSeq;
    private Sequence CloseSeq;
    private Vector2 bodyMinmumSize;
    private Vector2 headMinmumSize;
    private KeywordEntity entity;

    //public void Open()
    //{
    //    if (!gameObject.activeInHierarchy) return;
    //    if (isKeywordSlotOpen)
    //    {
    //        return;
    //    }
    //    isKeywordSlotOpen = true;
    //    if (CloseSeq != null && CloseSeq.IsPlaying())
    //    {
    //        CloseSeq.Kill();
    //    }
    //    transform.SetAsLastSibling();
    //    body.rectTransform.DOKill();
    //    header.rectTransform.DOKill();
    //    //header.rectTransform.sizeDelta = headMinmumSize;
    //    //body.rectTransform.sizeDelta = bodyMinmumSize;
    //    titleLogo.color = new Color(1, 1, 1, 0);
    //    textTitle.text = "";

    //    float headerAnimTime = headerOpenTime * (1 - (header.rectTransform.sizeDelta.magnitude / headerDesireSize.magnitude));
    //    float bodyAnimTime = bodyOpenTime * (1 - (body.rectTransform.sizeDelta.magnitude / bodyDesireSize.magnitude));

    //    int headBlinkTime = (int)(headerAnimTime / 0.3f);
    //    int bodyBlinkTime = (int)(bodyAnimTime / 0.3f);
    //    header.rectTransform.DOSizeDelta(headerDesireSize, headerAnimTime).OnComplete(() => body.rectTransform.DOSizeDelta(bodyDesireSize, bodyAnimTime));
    //    //header.color = headBlinkTime >0? Color.black : Color.white;
    //    //body.color = bodyBlinkTime>0? Color.black : Color.white;

    //    // openSeq = DOTween.Sequence();
    //    //openSeq.Append(header.rectTransform.DOSizeDelta(headerDesireSize, headerAnimTime));
    //    ////openSeq.Join(header.DOColor(Color.white, headerAnimTime / headBlinkTime).SetLoops(headBlinkTime));
    //    //openSeq.Append(body.rectTransform.DOSizeDelta(bodyDesireSize, bodyAnimTime));
    //    ////openSeq.Join(body.DOColor(Color.white, bodyAnimTime / bodyBlinkTime).SetLoops(bodyBlinkTime));
    //    ////openSeq.Join(textTitle.DOText(text, textTitleOpenTime));
    //    ////openSeq.Join(titleLogo.DOFade(1, titleLogoFadeTime));
    //    //openSeq.OnComplete(() => { openSeq = null; });
    //    //openSeq.Play();
    //}
    //public void Close()
    //{
    //    if (!gameObject.activeInHierarchy) return;
    //    if (isDrag || !isKeywordSlotOpen)
    //    {
    //        return;
    //    }

    //    isKeywordSlotOpen = false;
    //    if (openSeq != null && openSeq.IsPlaying())
    //    {
    //        openSeq.Kill();
    //    }

    //    textTitle.text = "";
    //    titleLogo.color = new Color(1, 1, 1, 0);

   
    //    float headerAnimTime = headerOpenTime *(header.rectTransform.sizeDelta.magnitude / headerDesireSize.magnitude);
    //    float bodyAnimTime = bodyOpenTime * (body.rectTransform.sizeDelta.magnitude / bodyDesireSize.magnitude);
    //    //body.rectTransform.sizeDelta = bodyMinmumSize;
    //    //header.rectTransform.sizeDelta = headMinmumSize;
    //    body.rectTransform.DOSizeDelta(bodyMinmumSize, bodyAnimTime).OnComplete(()=>header.rectTransform.DOSizeDelta(headMinmumSize, headerAnimTime)) ;
    //    //header.rectTransform.DOSizeDelta(headMinmumSize, headerAnimTime);
    //    //CloseSeq = DOTween.Sequence();
    //    //CloseSeq.Append(body.rectTransform.DOSizeDelta(bodyMinmumSize, bodyAnimTime));
    //    //CloseSeq.Append(header.rectTransform.DOSizeDelta(headMinmumSize, headerAnimTime));
    //    //CloseSeq.OnComplete(() => { CloseSeq = null; });
    //    //CloseSeq.Play();
    //}

    public void SetKeywordsLength(int length) 
    {
        if (length <= 1) 
        {
            textTitle.gameObject.SetActive(false);
            titleLogo.gameObject.SetActive(false);
        }
        //headerDesireSize.x = SizeX;
        //bodyDesireSize.x = SizeX;
        //headMinmumSize = new Vector2(0, headerDesireSize.y);
        //bodyMinmumSize = new Vector2(bodyDesireSize.x, 0);
        //header.rectTransform.sizeDelta = headMinmumSize;
        //body.rectTransform.sizeDelta = bodyMinmumSize;
        //panelTrs.anchoredPosition = new Vector2(-(bodyDesireSize.x / 2), headerDesireSize.y);
    }

    public override void Init()
    {
        mask.sizeDelta = Vector2.zero;
    }
    public void RegisterEntity(KeywordEntity entity) 
    {
        this.entity = entity;
    }

    public void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(entity.transform.position);
    }
    public void DragOn() => isDrag = true;
    public void DragOff() 
    {
        isDrag = false;
        Close();
    }
    public void Open()
    {
        if (!gameObject.activeInHierarchy) return;
        if (isKeywordSlotOpen)
        {
            return;
        }
        isKeywordSlotOpen = true;
        transform.SetAsLastSibling();
        mask.DOKill();
        Vector2 size = keywordSlotLayout.sizeDelta;
        size.x += 30;
        size.y += 80;
        float animTime = OPEN_ANIM_TIME * (1 - (mask.sizeDelta.magnitude / size.magnitude));
        mask.DOSizeDelta(size, animTime);
    }
    public void Close()
    {
        if (!gameObject.activeInHierarchy) return;

        if (isDrag || !isKeywordSlotOpen)
        {
            return;
        }
        isKeywordSlotOpen = false;
        mask.DOKill();
        mask.DOSizeDelta(Vector2.zero, ClOSE_ANIM_TIME);
    }
    private void OnDestroy()
    {
        //if (openSeq != null && openSeq.IsPlaying())
        //{
        //    openSeq.Kill();
        //}
        //if (CloseSeq != null && CloseSeq.IsPlaying())
        //{
        //    CloseSeq.Kill();
        //}
        mask.DOKill();
    }
  
}
