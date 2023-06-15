using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField]
    private Define.SOUND sound;
    public void LoadSceneSound()
    {
        Managers.Sound.PlaySFX(sound);
    }    
}
