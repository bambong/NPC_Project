using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    private Transform target;
    [SerializeField]
    private float dumpingAmount = 1f;

    private Vector3 diffPosToTarget;


    public void SetTarger(Transform target) 
    {
        this.target = target;
        diffPosToTarget = transform.position - target.position;
    }

    private void LateUpdate()
    {
        var pos = Vector3.Lerp(transform.position,target.position + diffPosToTarget,dumpingAmount * Time.deltaTime);
        transform.position = pos;
    }
}