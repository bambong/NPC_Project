using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNodeController : MonoBehaviour
{
    [SerializeField]
    private TutorialNodePanelController tutorialNodePanel;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private PauseTutorialData pauseTutorialData;

    public PauseTutorialData PauseTutorialData { get => pauseTutorialData;}

    private void Start()
    {
        icon.sprite = pauseTutorialData.iconImage;
        text.text = pauseTutorialData.iconTitle;
    }

    public void OnNodeButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        tutorialNodePanel.SelectCurrentNode(this);
    }
   
}
