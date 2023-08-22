using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePartController : MonoBehaviour
{
    [SerializeField]
    private Vector3 originPos;
    [SerializeField]
    private Vector3 originRot;

    [SerializeField]
    private float animTime = 1f;
    private bool isSuccess = false;

    [ContextMenu("Success")]
    public void OnSuccess() 
    {
        if (isSuccess) 
        {
            return;
        }
        isSuccess = true;
        transform.DOLocalMove(originPos, animTime);
        transform.DOLocalRotate(originRot, animTime);
    }
}
