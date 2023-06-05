using UnityEngine;
using System.Collections;

public class SecurityDoorController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private bool isOpen = false;
    private bool isStay = true;

    private Coroutine minOpenCo;
    private float curOpenTime = 0;

    private float MIN_OPEN_TIME = 0.5f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            Debug.Log("Open");
            isStay = true;
            OpenDoor();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStay = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            Debug.Log("Close");
            isStay = false;
            CloseDoor();
        }
    }


    private void OpenDoor()
    {
        if (isOpen) 
        {
            return;
        }
        if(minOpenCo != null)
        {
            StopCoroutine(minOpenCo);
        }
        isOpen = true;
        animator.SetBool("isOpen", true);
        curOpenTime = 0;
        Managers.Sound.PlaySFX(Define.SOUND.OpenDoor);
        minOpenCo = StartCoroutine(WaitAndCloseDoor());
        
        //float animationDuration = animator.GetCurrentAnimatorClipInfo(0).Length;
        //StartCoroutine(WaitAndCloseDoor(animationDuration));
    }

    private void CloseDoor()
    {

        if(minOpenCo != null) 
        {
            return;
        }
        if (!isOpen)
        {
            return;
        }

        isOpen = false;
        animator.SetBool("isOpen", false);
        //float animationDuration = animator.GetCurrentAnimatorClipInfo(0).Length;
        //StartCoroutine(WaitAndCloseDoor(animationDuration));
    }

    IEnumerator WaitAndCloseDoor()
    {
        while(curOpenTime <= MIN_OPEN_TIME ) 
        {
            if (!isStay) 
            {
                curOpenTime += Time.deltaTime;
            }
            else 
            {
                curOpenTime = 0;
            }
            yield return null;
        }
        curOpenTime = 0;
        minOpenCo = null;
        CloseDoor();
    }
}
