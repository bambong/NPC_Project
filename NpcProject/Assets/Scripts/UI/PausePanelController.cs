using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PausePanelController : BasePanelController
{

    [SerializeField]
    private CanvasGroup mainCanvasGroup;

    [SerializeField]
    private PauseMenuPanelController pauseMenuPanel;

    [SerializeField]
    private GameSettingPanelController gameSettingPanel;

    [SerializeField]
    private PauseTutorialPanelController tutorialPanelController;

    [SerializeField]
    private KeyMenuPanelController keyMenuPanelController;

    private BasePanelController currentPanel;

    private bool isTransition;
    private readonly float TRANSITION_TIME = 0.7f;

    public bool IsTransition { get => isTransition || gameSettingPanel.IsTransition; }
    public PauseMenuPanelController PauseMenuPanel { get => pauseMenuPanel;  }
    public PauseTutorialPanelController TutorialPanelController { get => tutorialPanelController; }
    public KeyMenuPanelController KeyMenuPanelController { get => keyMenuPanelController; }

    public override void Init()
    {
        DontDestroyOnLoad(panel.gameObject);
        panel.gameObject.SetActive(false);
        gameSettingPanel.Init();
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        pauseMenuPanel.Open();
        currentPanel = pauseMenuPanel;

    }

    protected override void OnClose()
    {
        base.OnClose();
    }
    public void OnTutorialButtonActive() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        ChangeMainPanel(tutorialPanelController);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnGameSettingButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        ChangeMainPanel(gameSettingPanel);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnResumeButtonActive() 
    {
        Managers.Game.SetStateNormal();
        EventSystem.current.SetSelectedGameObject(null);
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
    }
    public void OnQuitButtonActive() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        Application.Quit();
    }

    public void ChangeToMainMenuPanel() 
    {
        ChangeMainPanel(pauseMenuPanel);
    }
    public void ChangeMainPanel(BasePanelController panel)
    {
        Debug.Log(panel);

        if (isTransition) 
        {
            return;
        }
        isTransition = true;
        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.Append(mainCanvasGroup.DOFade(0, TRANSITION_TIME/2));
        seq.AppendCallback(()=>
        {
            currentPanel.Close();
            panel.Open();
            currentPanel = panel;
        });
        seq.Append(mainCanvasGroup.DOFade(1, TRANSITION_TIME / 2));
        seq.OnComplete(() => isTransition = false);
        
        seq.Play();
    }

    public void KeyMenusButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        ChangeMainPanel(keyMenuPanelController);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
