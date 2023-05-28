using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLimit : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private Vector2 minXZLimits;
    [SerializeField]
    private Vector2 maxXZLimits;

    private void Start()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = virtualCamera.transform.position;
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, minXZLimits.x, maxXZLimits.x);
        cameraPosition.z = Mathf.Clamp(cameraPosition.z, minXZLimits.y, maxXZLimits.y);
        virtualCamera.transform.position = cameraPosition;
    }
}
