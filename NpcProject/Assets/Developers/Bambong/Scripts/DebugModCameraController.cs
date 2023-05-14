using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DebugModCameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cam;
    [SerializeField]
    private float moveSpeed = 10;
    [SerializeField]
    private Vector2 clampX;
    [SerializeField]
    private Vector2 clampY;

    private float bumpValue = 50f;

    private DebugModCameraUiController debugModCameraUiController;
    private bool isDebugMod = false;

    private void Awake()
    {
        debugModCameraUiController = Managers.UI.MakeSceneUI<DebugModCameraUiController>(null, "ArrowsCanvas");
    }

    public bool EnterDebugMod() 
    {
        if(isDebugMod) 
        {
            return false;
        }
        isDebugMod = true;
        var camEvent = new CameraSwitchEvent(new CameraInfo(cam));
        camEvent.OnComplete(() => {
            debugModCameraUiController.EnterDebugMode();
            StartCoroutine(MoveUpdate());
            });
        camEvent.Play();
        return true;
    }
    public void ExitDebugMod() 
    {
        debugModCameraUiController.ExitDebugMode();
        isDebugMod = false;
    }
    private IEnumerator MoveUpdate()
    {
        while(isDebugMod) 
        {

            var mousePos = Input.mousePosition;
            float hor = 0;
            if(Screen.width - bumpValue <= mousePos.x) 
            {
                hor = 1;
            }
            else if (bumpValue >= mousePos.x) 
            {
                hor = -1;
            }

            var forward = Camera.main.transform.forward;
            forward.y = 0;
            var angle = -1 * Vector3.Angle(Vector3.forward, forward);
           

            var ver = 0;
            if (Screen.height - bumpValue <= mousePos.y)
            {
                ver = 1;
            }
            else if (bumpValue >= mousePos.y)
            {
                ver = -1;
            }

            var moveVec = new Vector3(hor,ver,0).normalized;
            var pos = transform.position;
            var speed = moveSpeed * Time.deltaTime;
            moveVec = Quaternion.AngleAxis(angle, Vector3.up) * moveVec.normalized;
            pos += moveVec * speed;
            pos.x = Mathf.Clamp(pos.x,clampX.x,clampX.y);
            pos.y = Mathf.Clamp(pos.y,clampY.x,clampY.y);
            transform.position = pos;

            debugModCameraUiController.ButtonStateCheckUpdate(pos, clampX, clampY, hor, ver);

            yield return null;
        
        }
    }
}
