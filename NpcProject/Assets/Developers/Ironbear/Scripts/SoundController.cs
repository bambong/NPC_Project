using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public GameObject Go => throw new System.NotImplementedException();

    [SerializeField]
    private GameObject soundObject;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        bgmSource = transform.Find("BgmSource").GetComponent<AudioSource>();
        sfxSource = transform.Find("SfxSource").GetComponent<AudioSource>();
    }

    public void BgmPlay(AudioClip bgmClip = null)
    {        
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SfxPlay(AudioClip sfxClip = null)
    {
        sfxSource.clip = sfxClip;
        sfxSource.PlayOneShot(sfxClip);
    }
}
