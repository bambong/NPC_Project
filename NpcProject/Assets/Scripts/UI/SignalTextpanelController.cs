using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignalTextpanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private bool isOpen;
    private const float OPEN_ANIM_TIME = 0.5f;
    private const float CLOSE_ANIM_TIME = 0.5f;
    private const float CHAR_ANIM_TIME = 0.2f;

    [ContextMenu("Open")]
    public void Open()
    {
        if (isOpen)
        {
            return;
        }
        isOpen = true;
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
        canvasGroup.DOFade(0, animTime).SetUpdate(true);
    }

    public void SetText(string str) 
    {
        text.DOText(str, str.Length * CHAR_ANIM_TIME).SetEase(Ease.Linear);
    }
}