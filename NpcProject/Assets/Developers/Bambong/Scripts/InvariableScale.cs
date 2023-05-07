using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class InvariableScale : MonoBehaviour
{
    [SerializeField]
    private Vector3 origin = Vector3.one;
    void Update()
    {
        var parentScale = transform.parent.lossyScale;
        transform.localScale = new Vector3(origin.x/ parentScale.x,origin.y/parentScale.y,origin.z/parentScale.z)  ;        
    }
}
