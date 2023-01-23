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



    private bool isDebugMod = false;
 
    public bool EnterDebugMod() 
    {
        if(isDebugMod) 
        {
            return false;
        }
        isDebugMod = true;
        var camEvent = new CameraSwitchEvent(new CameraInfo(cam));
        camEvent.OnComplete(() => {
            StartCoroutine(MoveUpdate());
            });
        camEvent.Play();
        return true;
    }
    public void ExitDebugMod() 
    {
        isDebugMod = false;
    }
    private IEnumerator MoveUpdate()
    {
        while(isDebugMod) 
        {
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");


            var moveVec = new Vector3(hor,ver,0).normalized;
            var pos = transform.position;
            var speed = moveSpeed * Time.deltaTime;
            pos += moveVec * speed;
            pos.x = Mathf.Clamp(pos.x,clampX.x,clampX.y);
            pos.y = Mathf.Clamp(pos.y,clampY.x,clampY.y);
            transform.position = pos;

            yield return null;
        
        }
    }
}
