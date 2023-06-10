using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using URPGlitch.Runtime.DigitalGlitch;
using UnityEngine.Rendering.Universal;
using System;

public class DebugModGlitchEffectController : UI_Base
{
    public GameObject effect;
    //[SerializeField]
    //DigitalGlitchVolume glitch;
    //Tonemapping tonemapping;

    private Volume volume;
    private bool isPlaying = false;
    private readonly float EffectTime = 0.3f;
    public bool IsPlaying { get => isPlaying;}

    private void Start()
    {
        Managers.Scene.OnSceneUnload += () => { StopAllCoroutines(); };
    }

    public void EnterDebugMod(Action completeAction = null)
    {
        volume.weight = 0;
        effect.SetActive(true);
        StartCoroutine(GlitchOnEffect(completeAction));
    }

    public void ExitDebugMod(Action completeAction = null)
    {
        StartCoroutine(GlitchOffEffect(completeAction));
    }

    public override void Init()
    {
        volume = gameObject.GetComponent<Volume>();
        //DigitalGlitchVolume tmp;
       // if(volume.profile.TryGet<DigitalGlitchVolume>(out tmp))
       // {
       //     glitch = tmp;
       // }
       //// Tonemapping tone;
        //if (volume.profile.TryGet<Tonemapping>(out tone))
        //{
        //    tonemapping = tone;
        //}
        Managers.UI.SetCanvas(gameObject);
    }

    public void PlayOnlyEffectGlitch()
    {
        effect.gameObject.SetActive(true);
        StartCoroutine(OnlyEffect());
    }

    IEnumerator OnlyEffect() 
    {
       // var intensity = glitch.intensity;
        //intensity.value = 1;
        //glitch.intensity = intensity;
        float progress = 0;
        var thirdTime = (EffectTime / 3);
        var effectFactor = 1 / thirdTime;
        while (progress < 1)
        {
            progress += Time.deltaTime * effectFactor;
            volume.weight = progress;
            yield return null;

        }
        progress = 1;
        volume.weight = 1;
        // tonemapping.active = true;
        yield return new WaitForSeconds(thirdTime);
        while (progress > 0)
        {
            progress -= Time.deltaTime * effectFactor;
         //   intensity.value = progress;
          //  glitch.intensity = intensity;
            yield return null;
        }
        //intensity.value = 0;
        //glitch.intensity = intensity;
    }
    IEnumerator GlitchOnEffect(Action completeAction)
    {
        isPlaying = true;
        yield return null;
       //var intensity = glitch.intensity;
       // intensity.value = 1;
        //glitch.intensity = intensity;
        float progress = 0;
        var thirdTime = (EffectTime / 3);
        var effectFactor = 1/thirdTime ;
        while (progress < 1)
        {
            progress += Time.deltaTime * effectFactor;
            volume.weight = progress;
            yield return null;

        }
        progress = 1;
        volume.weight = 1;
        Managers.Keyword.EnterDebugMod();
       // tonemapping.active = true;
        yield return new WaitForSeconds(thirdTime);
        while(progress > 0)
        {
            progress -= Time.deltaTime * effectFactor;
            //intensity.value = progress;
            //glitch.intensity = intensity;
            yield return null;
        }
        //intensity.value = 0;
        //glitch.intensity = intensity;
        isPlaying = false;
        completeAction?.Invoke();

    }

    IEnumerator GlitchOffEffect(Action completeAction)
    {
        isPlaying = true;
     //   var intensity = glitch.intensity;
        float progress = 0;
        var thirdTime = (EffectTime / 3);
        var effectFactor = 1/thirdTime;
        while (progress < 1)
        {
            progress += Time.deltaTime * effectFactor;
            //intensity.value = progress;
            //glitch.intensity = intensity;
            yield return null;

        }
       
        //intensity.value = 1;
        //glitch.intensity = intensity;
        Managers.Keyword.ExitDebugMod();
        //tonemapping.active = false;
        yield return new WaitForSeconds(thirdTime);

        progress = 0;
        float a = 1;
        while (progress < 1)
        {
            progress += Time.deltaTime * effectFactor;
            a = Mathf.Lerp(a, 0, progress);
            volume.weight = a;
            yield return null;
        }
        volume.weight = 0;
        effect.SetActive(false);
        isPlaying = false;
        completeAction?.Invoke();
    }

}
