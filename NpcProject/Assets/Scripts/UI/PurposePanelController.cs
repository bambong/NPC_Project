using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurposePanelController : UI_Base
{

    [SerializeField]
    private Image panel;

    [SerializeField]
    private Color defaultPanelColor; 

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private TextMeshProUGUI purposeText;
    private bool isOpen;
    private const float OPEN_ANIM_TIME = 1f;
    private const float CLOSE_ANIM_TIME = 1f;
    private const float CHANGE_PURPOSE_ANIM_OPEN_TIME = 0.5f;
    private const float CHANGE_PURPOSE_ANIM_CLOSE_TIME = 0.5f;


    [ContextMenu("Open")]
    public void Open() 
    {
        if (isOpen) 
        {
            return;
        }
        isOpen = true;
        purposeText.text = Managers.Data.GetCurrentPurpose();
        canvasGroup.DOKill();
        var animTime = OPEN_ANIM_TIME * (1 - canvasGroup.alpha);
        canvasGroup.DOFade(1, animTime).SetUpdate(true);

    }

    [ContextMenu("Close")]
    public void Close()
    {
        if (!isOpen)
        {
            return;
        }
        isOpen = false;

        canvasGroup.DOKill();
        var animTime = CLOSE_ANIM_TIME * canvasGroup.alpha;
        canvasGroup.DOFade(0, CLOSE_ANIM_TIME).SetUpdate(true);
    }

    public void SetPurpose(string str) 
    {
        var seq = DOTween.Sequence();
        seq.Append(panel.DOColor(Color.white, CHANGE_PURPOSE_ANIM_OPEN_TIME));
        seq.AppendCallback(() => { purposeText.text = str; });
        seq.AppendInterval(0.2f);
        seq.Append(panel.DOColor(defaultPanelColor, CHANGE_PURPOSE_ANIM_CLOSE_TIME));
        seq.Play();
    }
    public void UpdatePurpose()
    {
        var data = Managers.Data.GetCurrentPurpose();
        if (data == string.Empty) 
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
    [ContextMenu("Test Purpose")]
    public void TestPurPose()
    {
        Managers.Data.UpdateProgress(Managers.Data.Progress +1);
    }

    public override void Init()
    {
        Open();
    }
}
