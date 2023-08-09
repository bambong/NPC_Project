using DG.Tweening;
using UnityEngine;

public class StartGameSettingPanelController : BasePanelController
{
    [SerializeField]
    private CanvasGroup innerCanvasGroup;
    [SerializeField]
    private StartSoundSettingPanelController soundSettingPanel;
    [SerializeField]
    private InputSettingPanelController inputSettingPanel;
    [SerializeField]
    private ResolutionSettingPanelController resolutionSettingPanel;

    private ButtonBasePanelController currentPanel;
    private bool isTransition;
    private readonly float TRANSITION_TIME = 0.7f;
    public bool IsTransition { get => isTransition || inputSettingPanel.IsPanel; }

    public override void Init()
    {
        base.Init();
        soundSettingPanel.Init();
        inputSettingPanel.Init();
        resolutionSettingPanel.Init();
    }

    //또잉~
    private void Start()
    {
        currentPanel = soundSettingPanel;
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        soundSettingPanel.Open();
        soundSettingPanel.SetSelected(true);
        inputSettingPanel.Close();
        inputSettingPanel.SetSelected(false);
        resolutionSettingPanel.Close();
        resolutionSettingPanel.SetSelected(false);
        currentPanel = soundSettingPanel;
    }
    public void OnStartSoundSettingButtonActive() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        ChangeMainPanel(soundSettingPanel); 
    }
    public void OnStartInputSettingButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        ChangeMainPanel(inputSettingPanel); 
    }

    public void OnStartResolutionSettingButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        ChangeMainPanel(resolutionSettingPanel);
    }

    public void ChangeMainPanel(ButtonBasePanelController panel)
    {
        if (isTransition)
        {
            return;
        }
        isTransition = true;
        currentPanel.SetSelected(false);
        panel.SetSelected(true);
        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.Append(innerCanvasGroup.DOFade(0, TRANSITION_TIME / 2));
        seq.AppendCallback(() =>
        {
            currentPanel.Close();
            panel.Open();
            currentPanel = panel;
        });
        seq.Append(innerCanvasGroup.DOFade(1, TRANSITION_TIME / 2));
        seq.OnComplete(() => isTransition = false);

        seq.Play();
    }
}
