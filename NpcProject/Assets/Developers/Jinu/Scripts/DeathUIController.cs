using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeathUIController : UI_Base
{
    private readonly float Y_POS_REVISION_AMOUNT = 2f;
    public override void Init()
    {
        this.gameObject.SetActive(false);
    }

    public void DeathUIOpen()
    {
      //  var parent = Managers.Game.Player.transform;
      //  var pos  = parent.position + Vector3.up * ((parent.GetComponent<Collider>().bounds.size.y / 2) + Y_POS_REVISION_AMOUNT);
     //   pos.z = Camera.main.transform.position.z + 20f;
     //   transform.rotation = Camera.main.transform.rotation;
    //    transform.position = pos;
        this.gameObject.SetActive(true);
    }
    public void DeathUIClose()
    {
        this.gameObject.SetActive(false);
    }

}
