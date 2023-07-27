using UnityEngine;

public class KeyMenuPanelController : BasePanelController
{
    [SerializeField]
    private PausePanelController pausePanelController;

    public override void Init()
    {
        panel.gameObject.SetActive(false);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
    }

    public void OnCloseButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        pausePanelController.ChangeToMainMenuPanel();
    }

    public void PlayButtonSound() => Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
}
