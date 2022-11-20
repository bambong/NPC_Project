using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraController : MonoBehaviour
{
    private Transform camera;
    private void Awake()
    {
        camera = Camera.main.transform;
    }
    void Update()
    {
        transform.LookAt(camera.transform);
    }
}
