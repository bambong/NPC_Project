using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    [SerializeField]
    private StartPausePanelController startPausePanelController;
    private LetterBoxPanelController letterBoxPanelController;
    public override void Clear()
    {
    }
    private void Awake()
    {
        letterBoxPanelController = Managers.UI.MakeCameraSpaceUI<LetterBoxPanelController>(1f, null, "LetterBoxPanel");
        startPausePanelController.Init();
        //letterBoxPanelController.Init();
        //letterBoxPanelController.AnimationLetterBoxIn();
    }
}
