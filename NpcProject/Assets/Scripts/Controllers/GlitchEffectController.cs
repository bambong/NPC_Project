using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlitchEffectController : MonoBehaviour
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
}
