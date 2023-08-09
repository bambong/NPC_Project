using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartPausePanelController : BasePanelController
{

    [SerializeField]
    private CanvasGroup mainCanvasGroup;

    [SerializeField]
    private StartGameSettingPanelController gameSettingPanel;

    [SerializeField]
    private GameObject backBtns;

    private BasePanelController currentPanel;

    private bool isTransition;
    private readonly float TRANSITION_TIME = 0.7f;

    public bool IsTransition { get => isTransition || gameSettingPanel.IsTransition; }


    public override void Init()
    {
        panel.gameObject.SetActive(false);
        gameSettingPanel.Init();
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        gameSettingPanel.Open();
        currentPanel = gameSettingPanel;

    }

    protected override void OnClose()
    {
        base.OnClose();
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

    public void BackUIOff()
    {
        backBtns.SetActive(false);
    }

    public void BackUIOn()
    {
        backBtns.SetActive(true);
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
}
