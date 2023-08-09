using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class PauseTutorialPanelController :BasePanelController
{
    [SerializeField]
    private PausePanelController pausePanelController;
    [SerializeField]
    private CanvasGroup tutorialCanvasGroup;
    [SerializeField]
    private TutorialDescriptionPanelController tutorialDescriptionPanel;
    [SerializeField]
    private TutorialNodePanelController tutorialNodePanel;
    [SerializeField]
    private Button backButton;

   
    private BasePanelController currentPanel;

    private bool isTransition;
    private readonly float TRANSITION_TIME = 0.7f;

    public bool IsTransition { get => isTransition || pausePanelController.IsTransition;  }

 
    protected override void OnOpen()
    {
        base.OnOpen();
        tutorialNodePanel.Open();
        currentPanel = tutorialNodePanel;
        tutorialDescriptionPanel.Close();
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(OnNodePanelBackButtonActive);
    }
    protected override void OnClose()
    {
        base.OnClose();
    }

    public void OnNodePanelBackButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        pausePanelController.ChangeToMainMenuPanel();
    }
    public void OnDescriptionPanelBackButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        OnNodePanelOpen();
    }

    public void OnDescriptionPanelOpen()
    {
        if (isTransition)
        {
            return;
        }
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(OnDescriptionPanelBackButtonActive);
        ChangeMainPanel(tutorialDescriptionPanel);
    }
    public void OnNodePanelOpen()
    {
        if (isTransition)
        {
            return;
        }
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(OnNodePanelBackButtonActive);
        ChangeMainPanel(tutorialNodePanel);
    }
    public void ChangeMainPanel(BasePanelController panel)
    {
        if (isTransition)
        {
            return;
        }
        isTransition = true;
        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(tutorialCanvasGroup.DOFade(0, TRANSITION_TIME / 2));
        seq.AppendCallback(() =>
        {
            currentPanel.Close();
            panel.Open();
            currentPanel = panel;
        });
        seq.Append(tutorialCanvasGroup.DOFade(1, TRANSITION_TIME / 2));
        seq.OnComplete(() => isTransition = false);
        seq.Play();
    }
}
