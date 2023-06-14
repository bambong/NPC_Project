using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SetListener : MonoBehaviour
{
    private StudioListener player;
    void Start()
    {
        player = GetComponent<StudioListener>();
    }
}
