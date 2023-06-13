using DG.Tweening;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignalPanelController : UI_Base
{
    [SerializeField]
    private SignalEventData signalEventData;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private TextMeshProUGUI talkText;

    private Dictionary<int, List<string>> signalEvents = new Dictionary<int, List<string>>();
    private bool isOpen;
    private const float OPEN_ANIM_TIME = 0.5f;
    private const float CLOSE_ANIM_TIME = 0.5f;
    private const float CHAR_ANIM_TIME = 0.1f;
    private const float LINE_WAIT_TIME = 0.5f;

    public bool IsOpen { get => isOpen; }

    public override void Init()
    {
        if(signalEventData == null) 
        {
            signalEventData = Resources.Load<SignalEventData>("Data/SignalData");
        }

        foreach (var item in signalEventData.SignalTalks)
        {
            signalEvents.Add(item.eventId, item.texts);
        }
    }

    [ContextMenu("Open")]
    public void Open(int id)
    {

        if (!signalEvents.ContainsKey(id))
        {
            Debug.LogError("잘못된 SignalEvent 아이디 입력");
            return;
        }

        if (IsOpen)
        {
            return;
        }

        if (Managers.Game.Player.PurposePanel.IsOpen)
        {
            Managers.Game.Player.PurposePanel.Close(()=> { Open(id); });
            return;
        }

        isOpen = true;
        talkText.text = string.Empty;

        canvasGroup.DOKill();
        var animTime = OPEN_ANIM_TIME * (1 - canvasGroup.alpha);
        canvasGroup.DOFade(1, animTime).OnComplete(() => ProgressEvent(id,0));

    }

    [ContextMenu("Close")]
    public void Close()
    {
        if (!IsOpen)
        {
            return;
        }
        isOpen = false;

        canvasGroup.DOKill();
        var animTime = CLOSE_ANIM_TIME * canvasGroup.alpha;
        canvasGroup.DOFade(0, animTime).OnComplete(() => 
        {
            if (Managers.Game.Player != null) 
            {
                Managers.Game.Player.PurposePanel.Open();
            }
        }
      );
    }
    public void ProgressEvent(int id, int index) 
    {
        if(index >= signalEvents[id].Count) 
        {
            Close();
            return;
        }
        talkText.text = string.Empty;
        var text = signalEvents[id][index];
        
        Sequence seq = DOTween.Sequence();
        seq.Append(talkText.DOText(text, text.Length * CHAR_ANIM_TIME).SetEase(Ease.Linear));
        seq.AppendInterval(LINE_WAIT_TIME);
        seq.OnComplete(() => ProgressEvent(id, index + 1));
        seq.Play();
    }
}
