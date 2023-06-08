using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CapPanelController : MonoBehaviour
{
    public Speaker player;

    [SerializeField]
    private TMP_Text text1;

    private CanvasGroup capPanel;

    void Start()
    {
        capPanel = GetComponent<CanvasGroup>();
        UpdateName();

        capPanel.alpha = 0f;
        capPanel.DOFade(1f, 1f);
    }

    private void UpdateName()
    {
        string dialogue = "<Player>";
        dialogue = dialogue.Replace("<Player>", player.charName);
        text1.text = dialogue;
    }
}
