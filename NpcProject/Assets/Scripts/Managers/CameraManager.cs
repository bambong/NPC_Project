using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public struct CameraInfo
{
    public CinemachineVirtualCamera cam;
    public Transform target;

    public CameraInfo(CinemachineVirtualCamera cam,Transform target = null)
    {
        this.cam = cam;
        this.target = target;
    }
}

public class CameraSwitchEvent : GameEvent 
{
    private CameraInfo camInfo;
    public CinemachineVirtualCamera TargetCam { get => camInfo.cam; }
    public Transform TargetTrs { get => camInfo.target; }
    public CameraInfo CamInfo { get => camInfo;  }

    public CameraSwitchEvent(CameraInfo info)
    {
        camInfo = info;
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
    private CameraInfo prevCamInfo;
    private CameraInfo curCamInfo;
    
    public ICinemachineCamera CurActiveCam 
    {
        get => brain.ActiveVirtualCamera;
    }

    public void Init() 
    {
        brain = Util.GetOrAddComponent<CinemachineBrain>(Camera.main.gameObject);
    }
    public void SwitchPrevCamera() 
    {
        EnterSwitchCamera(new CameraSwitchEvent(prevCamInfo));
    }
    public void InitCamera(CameraInfo info) 
    {
        SetCurCamInfo(info);
        curCamInfo.cam.gameObject.SetActive(true);

    }
    private void SetCurCamInfo(CameraInfo info) 
    {
        curCamInfo = info;
        if(curCamInfo.target != null) 
        {
            curCamInfo.cam.Follow = curCamInfo.target;
            curCamInfo.cam.LookAt = curCamInfo.target;
        }
        curCamInfo.cam.LookAt = null;
    }
    public bool EnterSwitchCamera(CameraSwitchEvent switchEvent) 
    {
        if(switchEvent.TargetCam == curCamInfo.cam) 
        {
            return false;
        }

        prevCamInfo = curCamInfo;
        curCamInfo.cam.gameObject.SetActive(false);
       
        SetCurCamInfo(switchEvent.CamInfo);

        curCamInfo.cam.gameObject.SetActive(true);
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
