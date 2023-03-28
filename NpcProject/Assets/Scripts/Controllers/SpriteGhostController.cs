using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Platform;
using UnityEngine;

public class SpriteGhostController : GhostEffectController
{
    [SerializeField]
    private SpriteRenderer playerSpriteRender;
    
    [SerializeField]
    private float interval = 0.2f;
    [SerializeField]
    private float fadeTime = 0.2f;

    [SerializeField]
    private float alpha = 0.3f;

    private float curtime = 0;
    private readonly string SOMBRA_NAME = "Sombra";

    private void LateUpdate()
    {
        curtime += Time.deltaTime;
        if(curtime > interval) 
        {
            curtime = 0;
            CreateSombra();
        }
    }
    private void CreateSombra() 
    {
        var sombra = Managers.Resource.Instantiate(SOMBRA_NAME).GetComponent<SombraController>();
        var color = colorPicker.CurrentColor;
        color.a = alpha;
        sombra.Init(playerSpriteRender.sprite, color, fadeTime);
        sombra.transform.position = transform.position;
        sombra.transform.rotation = transform.rotation;
        sombra.transform.localScale = transform.lossyScale;

    }

    public override void Open()
    {
        curtime = interval;
        enabled = true;
    }

    public override void Close()
    {
        enabled = false;
    }
}
