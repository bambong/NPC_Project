using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    public override void Clear()
    {
    }
    private void Awake()
    {
        Managers.Sound.PlaySFX(Define.SOUND.NextChapter);
    }
}
