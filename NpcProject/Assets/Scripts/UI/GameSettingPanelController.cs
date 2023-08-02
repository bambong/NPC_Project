using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingPanelController : BasePanelController
{
    [SerializeField]
    private CanvasGroup innerCanvasGroup;
    [SerializeField]
    private SoundSettingPanelController soundSettingPanel;
    [SerializeField]
    private InputSettingPanelController inputSettingPanel;

    private ButtonBasePanelController currentPanel;
    private bool isTransition;
    private readonly float TRANSITION_TIME = 0.7f;
    public bool IsTransition { get => isTransition || inputSettingPanel.IsPanel; }

    public override void Init()
    {
        base.Init();
        soundSettingPanel.Init();
        inputSettingPanel.Init();
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        soundSettingPanel.Open();
        soundSettingPanel.SetSelected(true);
        inputSettingPanel.Close();
        inputSettingPanel.SetSelected(false);
        currentPanel = soundSettingPanel;
    }
    public void OnSoundSettingButtonActive() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        ChangeMainPanel(soundSettingPanel); 
    }
    public void OnInputSettingButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        ChangeMainPanel(inputSettingPanel); 
    }

    public void ChangeMainPanel(ButtonBasePanelController panel)
    {
        if (isTransition)
        {
            return;
        }
        if(currentPanel == panel) 
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
