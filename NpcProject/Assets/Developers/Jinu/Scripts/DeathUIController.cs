using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeathUIController : UI_Base

{    public override void Init()
    {
        this.gameObject.SetActive(false);
    }

    public void DeathUIOpen()
    {
        this.gameObject.SetActive(true);
    }
    public void DeathUIClose()
    {
        this.gameObject.SetActive(false);
    }

}
