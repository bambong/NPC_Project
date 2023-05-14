using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalScript : MonoBehaviour, IInteraction
{
    [SerializeField]
    private Define.Scene targetScene;
    public GameObject Go => gameObject;

    public void OnInteraction()
    {
        Managers.Scene.LoadScene(targetScene);
    }
}
