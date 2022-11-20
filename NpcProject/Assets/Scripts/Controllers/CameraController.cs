using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{

    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float dumpingAmount = 1f;

    
        private Vector3 diffPosToTarget;
        private void Awake()
        {
            diffPosToTarget = transform.position - target.position;
        }

        private void LateUpdate()
        {
            var pos = Vector3.Lerp(transform.position,target.position + diffPosToTarget,dumpingAmount * Time.deltaTime);
            transform.position = pos;
        }
    }

}