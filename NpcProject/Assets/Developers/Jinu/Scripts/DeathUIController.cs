using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class DeathUIController : UI_Base
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Camera cam;
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
        canvas.worldCamera = cam;
        
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.renderPostProcessing = false;
        cameraData.cameraStack.Add(cam);
        this.gameObject.SetActive(true);
    }
    public void DeathUIClose()
    {
        this.gameObject.SetActive(false);
    }

}
