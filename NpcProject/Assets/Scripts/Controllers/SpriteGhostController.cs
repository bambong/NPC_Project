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
    
    private void FixedUpdate()
    {
        curtime += Time.fixedDeltaTime;
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
        sombra.hideFlags = HideFlags.HideInInspector;
        color.a = alpha;
        sombra.Init(playerSpriteRender.sprite, color, fadeTime);

        var pos = transform.position + (Camera.main.transform.forward.normalized * 0.01f);
        sombra.transform.position = pos;
        sombra.transform.rotation = transform.rotation;
        sombra.transform.localScale = transform.lossyScale;

    }

    private void PlayWalkSFX()
    {
        Managers.Sound.PlaySFX(Define.SOUND.WalkPlayer);
    }

    private void PlayRunSFX()
    {
        Managers.Sound.PlaySFX(Define.SOUND.RunPlayer);
    }

    public override void Open()
    {
        //curtime = interval;
        enabled = true;
    }

    public override void Close()
    {
        enabled = false;
    }
}
