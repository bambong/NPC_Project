using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlitchEffectController : UI_Base
{
    public GameObject effect;

    public void OnGlitch()
    {
        effect.SetActive(true);
        Debug.Log("Glitch On");
    }

    public void OffGlitch()
    {
        effect.SetActive(false);
        Debug.Log("Glitch Off");
    }

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject);
    }
}
