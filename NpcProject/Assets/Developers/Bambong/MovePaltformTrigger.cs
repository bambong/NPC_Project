using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePaltformTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform invariableArea;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            other.transform.SetParent(invariableArea);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.transform.parent == invariableArea) 
        {
            other.transform.SetParent(null);
        }
    }
}
