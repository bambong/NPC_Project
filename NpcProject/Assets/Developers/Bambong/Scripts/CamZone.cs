using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider))]
public class CamZone : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera verCamera;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) 
        {
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
            verCamera.m_LookAt = other.transform;
            verCamera.m_Follow = other.transform;
            verCamera.gameObject.SetActive(true);
        }
    }
    

}
