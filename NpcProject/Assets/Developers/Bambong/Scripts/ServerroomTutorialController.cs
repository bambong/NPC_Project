using UnityEngine;

public class ServerroomTutorialController : BaseTutorialController
{
    [Header("게임동안 한번만 실행할 것인지")]
    [SerializeField]
    private bool isEventOnce;

    public override bool Open()
    {
        if (isEventOnce && Managers.Data.IsClearEvent(guId)) 
        {
            return false;
        }
        return base.Open();
    }

    protected override void OnOpen()
    {
        Managers.Game.RetryPanel.CloseResetButton();
        Managers.Game.SetStateTutorial();
    }
    protected override void OnClose()
    {
        Managers.Game.RetryPanel.OpenResetButton();
        Managers.Game.SetStateNormal();
    }

    public void ClearGuIdEvent() 
    {
        Managers.Data.ClearEvent(guId);
    }
}
