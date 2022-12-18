using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMatchCheckController : MonoBehaviour
{

    private float CLEAR_DISTANCE = 0.5f;
    private float CLEAR_ROTATE = 10f;
    private Transform curTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interaction")) 
        {
            curTarget = other.transform;
        }   
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interaction") && other.transform == curTarget)
        {
            curTarget = null;
        }
    }
    public bool IsClear() 
    {
        if(curTarget == null) 
        {
            return false;
        }

        var dis = (curTarget.transform.position - transform.position).magnitude;
        if ( dis > CLEAR_DISTANCE) 
        {
            return false;
        }

        var rot = Quaternion.Angle(transform.rotation, curTarget.rotation);
        if( rot > CLEAR_ROTATE) 
        {
            return false;
        }

        return true;
    }

    public void Clear() 
    {
        curTarget.transform.position = transform.position;
        curTarget.transform.rotation = transform.rotation;
        gameObject.SetActive(false);
    }


}
