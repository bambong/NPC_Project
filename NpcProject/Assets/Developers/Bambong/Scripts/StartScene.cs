using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    private LetterBoxPanelController letterBoxPanelController;
    public override void Clear()
    {
    }
    private void Awake()
    {
        letterBoxPanelController = Managers.UI.MakeCameraSpaceUI<LetterBoxPanelController>(1f, null, "LetterBoxPanel");
        //letterBoxPanelController.Init();
        //letterBoxPanelController.AnimationLetterBoxIn();
    }
}
