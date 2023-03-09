using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalScript : MonoBehaviour, IInteraction
{
    public GameObject Go => gameObject;

    public void OnInteraction()
    {
        Managers.Scene.LoadScene(Define.Scene.Clear);
    }
}
