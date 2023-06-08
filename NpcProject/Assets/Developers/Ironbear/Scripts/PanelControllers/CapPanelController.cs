using UnityEngine;
using DG.Tweening;

public class CapPanelController : MonoBehaviour
{
    private CanvasGroup capPanel;

    void Start()
    {
        capPanel = GetComponent<CanvasGroup>();

        capPanel.alpha = 0f;
        capPanel.DOFade(1f, 1f);
    }
}
