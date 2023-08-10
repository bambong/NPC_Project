using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PurposeData
{
    public string title;
    [TextArea]
    public string description;
}

public class PurposePanelController : UI_Base
{

    [SerializeField]
    private Image panel;

    [SerializeField]
    private Color defaultPanelColor; 

    [SerializeField]
    private CanvasGroup canvasGroup;


    [SerializeField]
    private TextMeshProUGUI purposeTitle;
    [SerializeField]
    private TextMeshProUGUI purposeText;

    
    private bool isOpen;
    private Color originPanelColor;
    private const float OPEN_ANIM_TIME = 0.5f;
    private const float CLOSE_ANIM_TIME = 0.5f;
    private const float CHANGE_PURPOSE_ANIM_OPEN_TIME = 0.3f;
    private const float CHANGE_PURPOSE_ANIM_CLOSE_TIME = 0.5f;
   
    public bool IsOpen { get => isOpen; }

    [ContextMenu("Open")]
    public void Open() 
    {
        if (isOpen) 
        {
            return;
        }
        var temp = Managers.Data.GetCurrentPurpose();
        if (temp == null) 
        {
            return;
        }
        isOpen = true;
        panel.color = originPanelColor;
        UpdateData(temp);
        canvasGroup.DOKill();
        var animTime = OPEN_ANIM_TIME * (1 - canvasGroup.alpha);
        canvasGroup.DOFade(1, animTime).SetUpdate(true);

    }

    private void UpdateData(PurposeData data) 
    {
        purposeTitle.text = $"[{data.title}]";
        purposeText.text = data.description != string.Empty ? $"- {data.description}" : string.Empty;
    }

    [ContextMenu("Close")]
    public void Close(Action onComplete  = null)
    {
        if (!isOpen)
        {
            return;
        }
        isOpen = false;

        canvasGroup.DOKill();
        var animTime = CLOSE_ANIM_TIME * canvasGroup.alpha;
        canvasGroup.DOFade(0, animTime).SetUpdate(true).OnComplete(() => { onComplete?.Invoke(); });
    }

    public void SetPurpose(PurposeData data) 
    {
        var seq = DOTween.Sequence();
        seq.Append(panel.DOColor(Color.white, CHANGE_PURPOSE_ANIM_OPEN_TIME));
        seq.AppendCallback(() => { UpdateData(data); });
        seq.AppendInterval(0.2f);
        seq.Append(panel.DOColor(defaultPanelColor, CHANGE_PURPOSE_ANIM_CLOSE_TIME));
        seq.Play();
    }
    public void UpdatePurpose()
    {
        var data = Managers.Data.GetCurrentPurpose();
        if (data == null) 
        {
            Close();
            return;
        }
        if (isOpen) 
        {
            SetPurpose(data);
            return;
        }
        
        Open();
    }
    public void ClearPurpose() 
    {
        var seq = DOTween.Sequence();
        seq.Append(panel.DOColor(Color.white, CHANGE_PURPOSE_ANIM_OPEN_TIME));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => {
            purposeText.text = string.Empty; });
        seq.Append(canvasGroup.DOFade(0, CLOSE_ANIM_TIME));
        seq.Play();
    }
    public override void Init()
    {
        originPanelColor = panel.color;
      //  Open();
    }
}
