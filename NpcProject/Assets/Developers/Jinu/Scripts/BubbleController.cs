using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [SerializeField]
    private BubbleUI bubbleUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bubbleUI.OpenBubbleUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            bubbleUI.CloseBubbleUI();
        }
    }
}
