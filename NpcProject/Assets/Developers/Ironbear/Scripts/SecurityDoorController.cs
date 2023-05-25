using UnityEngine;
using System.Collections;

public class SecurityDoorController : MonoBehaviour
{
    private Animator animator;

    private bool isOpen = false;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {            
            OpenDoor();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") )
        {           
            CloseDoor();
        }
    }


    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("isOpen", true);

        float animationDuration = animator.GetCurrentAnimatorClipInfo(0).Length;
        StartCoroutine(WaitAndCloseDoor(animationDuration));
    }

    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("isOpen", false);
        float animationDuration = animator.GetCurrentAnimatorClipInfo(0).Length;
        StartCoroutine(WaitAndCloseDoor(animationDuration));
    }

    IEnumerator WaitAndCloseDoor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
