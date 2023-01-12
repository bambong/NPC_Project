using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager 
{
    private CinemachineBrain brain;
    private CinemachineVirtualCamera prevCam;
    private CinemachineVirtualCamera curCam;
    
    public ICinemachineCamera CurActiveCam 
    {
        get => brain.ActiveVirtualCamera;
    }

    public void Init() 
    {
        
        brain = Util.GetOrAddComponent<CinemachineBrain>(Camera.main.gameObject);
    }
    public void InitCamera(CinemachineVirtualCamera cam,Transform target = null) 
    {
        curCam = cam;
        if(target != null)
        {
            curCam.m_LookAt = target;
            curCam.m_Follow = target;
        }
        curCam.gameObject.SetActive(true);
    }
    public void SwitchCamera(CinemachineVirtualCamera cam, Transform target = null) 
    {
        prevCam = curCam;
        curCam.gameObject.SetActive(false);
        curCam = cam;
        if(target != null)
        {
            curCam.m_LookAt = target;
            curCam.m_Follow = target;
        }
        curCam.gameObject.SetActive(true);
    }

}
