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
    private Vector3 erasePos;
    [SerializeField]
    private Vector3 eraseRot;

    [SerializeField]
    private float animTime = 1f;
    private bool isSuccess = false;

    [ContextMenu("Success")]
    public void OnSuccess() 
    {
        //if (isSuccess) 
        //{
        //    return;
        //}
        isSuccess = true;
        transform.DOLocalMove(originPos, animTime);
        transform.DOLocalRotate(originRot, animTime);
    }

    public void HideEvent()
    {
        transform.DOLocalMove(erasePos, animTime);
        transform.DOLocalRotate(eraseRot, animTime);
    }

    public void Hide()
    {
        this.transform.localPosition = erasePos;
        this.transform.rotation = Quaternion.Euler(eraseRot);
    }
}
