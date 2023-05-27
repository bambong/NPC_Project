using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalScript : MonoBehaviour, IInteraction
{
    [SerializeField]
    private Define.Scene targetScene;
    public GameObject Go => gameObject;
    public virtual bool IsInteractAble => true;
    public void OnInteraction()
    {
        Managers.Sound.PlaySFX(Define.SOUND.NextChapter);
        Managers.Scene.LoadScene(targetScene);
    }
}
