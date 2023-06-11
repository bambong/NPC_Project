using DynamicShadowProjector;
using ProjectorForLWRP;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;

public class PlayerShadowController : MonoBehaviour
{
    [SerializeField]
    private ProjectorForLWRP.ProjectorForLWRP projector;
    [SerializeField]
    private ShadowTextureRenderer shadowTextureRenderer;
    [SerializeField]
    private Transform rotateFactor;
    [SerializeField]
    private float shadowRotateSpeed  = 2f; 

    private Transform targetLight;
    private Coroutine targetTrackingCo;
    private Coroutine colorFadeCo;
    private const float CHANGE_ANIM_TIME = 0.2f;
    public void SetColor(Color color) 
    {
        shadowTextureRenderer.SetShadowColor(color);
    }

    public void SetLight(Transform transform) 
    {

        if( targetLight == transform) 
        {
            return;
        }

        bool isChange = false;
        if(targetTrackingCo != null) 
        {
            //if(colorFadeCo != null)
            //{
            //    StopCoroutine(colorFadeCo);
            //    colorFadeCo = null;
            //}
            //colorFadeCo = StartCoroutine(ColorFade());
            StopCoroutine(targetTrackingCo);
            targetTrackingCo = null;
            isChange = true;
        }


        targetLight = transform;
        targetTrackingCo = StartCoroutine(Tracking(isChange));
    }
    IEnumerator ColorFade()
    {
        float progress = 0;
        while (progress < CHANGE_ANIM_TIME)
        {
            progress += Time.deltaTime;
            SetColor(Color.Lerp(Color.black, Color.white , progress * (1/ CHANGE_ANIM_TIME)));
            yield return null;
        }
        SetColor(Color.white);
        var dir = (rotateFactor.position - targetLight.transform.position);
        dir.y = rotateFactor.position.y;
        var targetRot = Quaternion.LookRotation(dir.normalized);
        rotateFactor.rotation = targetRot;
         yield return null;
        progress = 0;
        while (progress < CHANGE_ANIM_TIME)
        {
            progress += Time.deltaTime;
            SetColor(Color.Lerp(Color.white, Color.black, progress * (1/ CHANGE_ANIM_TIME)));
            yield return null;
        }
        SetColor(Color.black);
        colorFadeCo = null;
    }
    IEnumerator Tracking(bool change) 
    {
        //if (change) 
        //{
        //    float progress = 0;
        //    while (progress < CHANGE_ANIM_TIME)
        //    {
        //        progress += Time.deltaTime;
        //        SetColor(Color.Lerp(Color.black, Color.white, progress * (1 / CHANGE_ANIM_TIME)));
        //        yield return null;
        //    }
        //    SetColor(Color.white);
        //    {
        //        var dir = (rotateFactor.position - targetLight.transform.position);
        //        dir.y = rotateFactor.position.y;
        //        var targetRot = Quaternion.LookRotation(dir.normalized);
        //        rotateFactor.rotation = targetRot;
        //    }
        //    yield return null;

        //    progress = 0;
        //    while (progress < CHANGE_ANIM_TIME)
        //    {
        //        progress += Time.deltaTime;
        //        SetColor(Color.Lerp(Color.white, Color.black, progress * (1 / CHANGE_ANIM_TIME)));
        //        var dir = (rotateFactor.position - targetLight.transform.position);
        //        dir.y = rotateFactor.position.y;
        //        var targetRot = Quaternion.LookRotation(dir.normalized);
        //        var amount = Time.deltaTime * shadowRotateSpeed;
        //        rotateFactor.rotation = Quaternion.Slerp(rotateFactor.rotation, targetRot, amount);
        //        yield return null;
        //    }
        //}
   
        while (targetLight != null) 
        {
            var dir = (rotateFactor.position - targetLight.transform.position);
            dir.y = rotateFactor.position.y;
            var targetRot = Quaternion.LookRotation(dir.normalized);
            var amount = Time.deltaTime * shadowRotateSpeed;
            rotateFactor.rotation = Quaternion.Slerp(rotateFactor.rotation, targetRot, amount);
            yield return null;
        }
        targetTrackingCo = null;
    }





}
