using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using URPGlitch.Runtime.DigitalGlitch;
public class GlitchEffectController : UI_Base
{
    public GameObject effect;
    [SerializeField]
    URPGlitch.Runtime.DigitalGlitch.DigitalGlitchVolume glitch;

    public void OnGlitch()
    {
        effect.SetActive(true);
        StartCoroutine(GlitchOnEffect());
        Debug.Log("Glitch On");
    }

    public void OffGlitch()
    {
        effect.SetActive(false);
        Debug.Log("Glitch Off");
    }

    public override void Init()
    {
        Volume volume = gameObject.GetComponent<Volume>();
        DigitalGlitchVolume tmp;
        if(volume.profile.TryGet<DigitalGlitchVolume>(out tmp))
        {
            glitch = tmp;
        }
        Managers.UI.SetCanvas(gameObject);
    }
    IEnumerator GlitchOnEffect()
    {
        var intensity = glitch.intensity;
        float progress = 0;
        while(progress < 1) 
        {
            progress += Time.deltaTime;
            intensity.value = progress;
            glitch.intensity = intensity;
            yield return null;
        
        }
        progress = 1;
        intensity.value = 1;
        glitch.intensity = intensity;
        while(progress > 0)
        {
            progress -= Time.deltaTime;
            intensity.value = progress;
            glitch.intensity = intensity;
            yield return null;
        }
        intensity.value = 0;
        glitch.intensity = intensity;
    }

}
