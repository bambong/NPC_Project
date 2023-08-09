using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNodePanelController : BasePanelController
{
    [SerializeField]
    private PauseTutorialPanelController pauseTutorialPanelController;
    [SerializeField]
    private TutorialDescriptionPanelController tutorialDescriptionPanel;
    public void SelectCurrentNode(TutorialNodeController node) 
    {
        if (pauseTutorialPanelController.IsTransition) 
        {
            return;
        }
        tutorialDescriptionPanel.SelectTutorialNode(node);
        pauseTutorialPanelController.OnDescriptionPanelOpen();
    }
}
