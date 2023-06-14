using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField]
    private Define.SOUND potal;
    public void ChapterSound()
    {
        Managers.Sound.PlaySFX(potal);
    }
}
