using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitchEvent : GameEvent 
{
    private CinemachineVirtualCamera targetCam;
    private Transform targetTrs;

    public CinemachineVirtualCamera TargetCam { get => targetCam; }
    public Transform TargetTrs { get => targetTrs; }

    public CameraSwitchEvent(CinemachineVirtualCamera targetCam,Transform targetTrs = null)
    {
        this.targetCam = targetCam;
        this.targetTrs = targetTrs;
    }

    public override void Play()
    {
        if (Managers.Camera.EnterSwitchCamera(this)) 
        {
            onStart?.Invoke();
        }
    }
    public void Complete()
    {
        onComplete?.Invoke();
    }

}

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
    public bool EnterSwitchCamera(CameraSwitchEvent switchEvent) 
    {
        if(switchEvent.TargetCam == curCam) 
        {
            return false;
        }

        prevCam = curCam;
        curCam.gameObject.SetActive(false);
        curCam = switchEvent.TargetCam;

        if(switchEvent.TargetTrs != null)
        {
            curCam.m_LookAt = switchEvent.TargetTrs;
            curCam.m_Follow = switchEvent.TargetTrs;
        }

        curCam.gameObject.SetActive(true);
        brain.StartCoroutine(SwitchEventCompleteCheck(switchEvent));
        return true;
    }
    private IEnumerator SwitchEventCompleteCheck(CameraSwitchEvent switchEvent) 
    {
       yield return null;

       while (brain.IsBlending)
       {
            yield return null;     
       }
       if(brain.IsLive(switchEvent.TargetCam))
       {
            switchEvent.Complete();
       }
    }

}
