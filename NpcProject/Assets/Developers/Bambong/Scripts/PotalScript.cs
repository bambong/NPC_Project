using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PotalScript : MonoBehaviour, IInteraction
{
    [SerializeField]
    private Define.Scene targetScene;

    [SerializeField]
    private UnityEvent onInteract;
    public GameObject Go => gameObject;
    public virtual bool IsInteractAble => true;
    public void OnInteraction()
    {
        onInteract?.Invoke();
        Managers.Sound.PlaySFX(Define.SOUND.Door);
        Managers.Scene.LoadScene(targetScene);
    }
}
