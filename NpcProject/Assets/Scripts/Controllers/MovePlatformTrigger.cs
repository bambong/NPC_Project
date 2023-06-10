using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform invariableArea;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (Managers.Game.Player.PlayerAncestor == transform.parent) 
            {
                return;
            }
            Managers.Game.Player.PlayerAncestor = transform.parent;
            other.transform.SetParent(invariableArea);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && other.transform.parent == invariableArea) 
        {

            if (Managers.Game.Player.PlayerAncestor != transform.parent)
            {
                return;
            }
            other.transform.SetParent(null);
            Managers.Game.Player.PlayerAncestor = null;
            other.transform.localScale = Vector3.one;
        }
    }
}
