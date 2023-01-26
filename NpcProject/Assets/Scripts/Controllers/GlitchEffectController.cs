using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using URPGlitch.Runtime.DigitalGlitch;
using UnityEngine.Rendering.Universal;

public class GlitchEffectController : UI_Base
{
    public GameObject effect;
    [SerializeField]
    DigitalGlitchVolume glitch;
    Tonemapping tonemapping;

    private Volume volume;
    private bool isPlaying = false;

    public bool IsPlaying { get => isPlaying;}

    public void OnGlitch()
    {
        volume.weight = 0;
        effect.SetActive(true);
        StartCoroutine(GlitchOnEffect());
    }

    public void OffGlitch()
    {
        StartCoroutine(GlitchOffEffect());
    }

    public override void Init()
    {
        volume = gameObject.GetComponent<Volume>();
        DigitalGlitchVolume tmp;
        if(volume.profile.TryGet<DigitalGlitchVolume>(out tmp))
        {
            glitch = tmp;
        }
        Tonemapping tone;
        if (volume.profile.TryGet<Tonemapping>(out tone))
        {
            tonemapping = tone;
        }
        Managers.UI.SetCanvas(gameObject);
    }
    IEnumerator GlitchOnEffect()
    {
        isPlaying = true;
        yield return null;
        var intensity = glitch.intensity;
        intensity.value = 1;
        glitch.intensity = intensity;
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            volume.weight = progress;
            yield return null;

        }
        progress = 1;
        volume.weight = 1;
        Managers.Keyword.EnterDebugMod();
        tonemapping.active = true;
        yield return new WaitForSeconds(1f);
        while(progress > 0)
        {
            progress -= Time.deltaTime;
            intensity.value = progress;
            glitch.intensity = intensity;
            yield return null;
        }
        intensity.value = 0;
        glitch.intensity = intensity;
        isPlaying = false;

    }

    IEnumerator GlitchOffEffect()
    {
        isPlaying = true;
        var intensity = glitch.intensity;
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            intensity.value = progress;
            glitch.intensity = intensity;
            yield return null;

        }
        progress = 1;
        intensity.value = 1;
        glitch.intensity = intensity;
        Managers.Keyword.ExitDebugMod();
        tonemapping.active = false;
        yield return new WaitForSeconds(1f);

        progress = 0;
        float a = 1;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            a = Mathf.Lerp(a, 0, progress);
            volume.weight = a;
            yield return null;
        }
        volume.weight = 0;
        effect.SetActive(false);
        isPlaying = false;
    }

}
