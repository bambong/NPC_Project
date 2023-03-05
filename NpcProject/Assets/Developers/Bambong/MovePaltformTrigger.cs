using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePaltformTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform invariableArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            other.transform.SetParent(invariableArea);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && other.transform.parent == invariableArea) 
        {
            other.transform.SetParent(null);
            other.transform.localScale = Vector3.one;
        }
    }
}
