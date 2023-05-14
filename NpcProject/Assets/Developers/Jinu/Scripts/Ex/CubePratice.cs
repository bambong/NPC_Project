using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePratice : MonoBehaviour
{
    float fadeOutDuration = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Stop());
            //Managers.Sound.bgmEmitter.EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //Managers.Sound.bgmEmitter.Stop();
            //Managers.Sound.ChangeBGM("Fade", 0.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Managers.Sound.bgmEmitter.Play();
        }
    }

    private IEnumerator Stop()
    {
        float startVolume;

        Managers.Sound.bgmEmitter.EventInstance.getParameterByName("Volume", out startVolume);
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeOutDuration)
        {
            float volume = Mathf.Lerp(1.0f, 0.0f, t);
            Managers.Sound.bgmEmitter.EventInstance.setParameterByName("Volume", volume);
            yield return null;
        }
        Managers.Sound.bgmEmitter.Stop();
    }
}
