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
    private Transform player;
    [SerializeField]
    private Vector3 playerMovePos;


    [SerializeField]
    private float panelfadeTim = 2f;
    [SerializeField]
    private float renderfadeTim = 2f;
    [SerializeField]
    private float interval = 1f;


 
    private readonly float EFFECT_START_TIME = 0.5f;
    private readonly float EFFECT_END_TIME = 0.3f;

    private void Start()
    {
        panelCanvasGroup.alpha = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.Append(panelCanvasGroup.DOFade(1, panelfadeTim));
        sequence.AppendInterval(interval);
        sequence.AppendCallback(() => { StartCoroutine(GlitchOnce()); });
        sequence.AppendInterval(EFFECT_START_TIME+ EFFECT_END_TIME);
        sequence.Append(panelCanvasGroup.DOFade(0, 1f));
      //  sequence.Append(buttonGroup.DOFade(1, 1f));
        sequence.OnComplete(() => { 
            Managers.Scene.LoadScene(Define.Scene.Chapter_02_Forest); 
        });
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
    public void QuitButton() 
    {
        Application.Quit();
    }


}
