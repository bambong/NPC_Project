using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeGameController : MonoBehaviour
{
    [SerializeField]
    private GameObject potal;

    public int clearCount = 0;

    public void Clear()
    {
        if (clearCount >= 2)
        {
            potal.gameObject.SetActive(true);
        }
        else
        {
            potal.gameObject.SetActive(false);
        }
    }
}
