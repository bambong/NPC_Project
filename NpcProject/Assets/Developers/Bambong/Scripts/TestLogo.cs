using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using URPGlitch.Runtime.AnalogGlitch;
using URPGlitch.Runtime.DigitalGlitch;

public class TestLogo : MonoBehaviour
{
    [SerializeField]
    private Volume glitchVol;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Vector3 camMovePos = Vector3.zero;

    [SerializeField]
    private CanvasGroup panelCanvasGroup;
    [SerializeField]
    private CanvasGroup renderCanvasGroup;
    [SerializeField]
    private CanvasGroup outLineCanvasGroup;
    [SerializeField]
    private Image glowImage;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Vector3 playerMovePos;

    [SerializeField]
    private CanvasGroup renderCanvasGroup2;
    [SerializeField]
    private float panelfadeTim = 2f;
    [SerializeField]
    private float renderfadeTim = 2f;
    [SerializeField]
    private float interval = 1f;

    private DigitalGlitchVolume digitalEffect;
    private AnalogGlitchVolume anologEffect;
    private ChromaticAberration chromaticEffect;
    private readonly float EFFECT_START_TIME = 0.5f;
    private readonly float EFFECT_END_TIME = 0.3f;
    private void Start()
    {
        renderCanvasGroup.alpha = 0;
        panelCanvasGroup.alpha = 0;
        outLineCanvasGroup.alpha = 1;
        renderCanvasGroup2.alpha = 0;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.Append(panelCanvasGroup.DOFade(1, panelfadeTim));
        sequence.Append(renderCanvasGroup.DOFade(1, renderfadeTim));
        sequence.AppendInterval(interval);
        sequence.AppendCallback(() => { StartCoroutine(GlitchOnce()); });
        sequence.AppendInterval(EFFECT_START_TIME+ EFFECT_END_TIME);
        sequence.Append(panelCanvasGroup.DOFade(0, 1.5f)); 
        sequence.Play();

    }
    IEnumerator GlitchOnce() 
    {
        glitchVol.weight = 0;
        float progress = 0;

        while(progress < 1/EFFECT_START_TIME) 
        {
            progress += Time.deltaTime * (1 / EFFECT_START_TIME);
            glitchVol.weight = progress;
            yield return null;
            
        }
        glitchVol.weight = 1;
        progress = 1;

        while (progress > 0)
        {
            progress -= Time.deltaTime * (1 / EFFECT_END_TIME);
            glitchVol.weight = progress;
            yield return null;
        }
        glitchVol.weight = 0;
    }



}
