using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUiController : UI_Base
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private RectTransform rectTransform;
    
    private readonly float ON_OFF_ANIM_TIME = 0.3f;
    private readonly float Y_POS_REVISION_AMOUNT = 2;
    private readonly string ON_STATE = "InteractionOn";
    private readonly string OFF_STATE = "InteractionOff";
    private readonly string IDLE_STATE = "InteractionIdle";
    private bool isOpening = false;

    public void Open()
    {
        gameObject.SetActive(true);
        StartCoroutine(OpenInteractionUi());
    }
    public void Open(Transform parent)
    {
        transform.position = parent.position + Vector3.up * ((parent.GetComponent<Collider>().bounds.size.y/2)+ Y_POS_REVISION_AMOUNT);
        transform.rotation  = Camera.main.transform.rotation;
        transform.SetParent(parent);
        gameObject.SetActive(true);
        StartCoroutine(OpenInteractionUi());
    }
    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
    public void Close() 
    {
        if(!gameObject.activeSelf) 
        {
            return;
        }
        StartCoroutine(CloseInteractionUi());
    } 
    private void PlayOnState() 
    {
        animator.Play(ON_STATE);
    }

    private void PlayIdleState()
    {
        animator.Play(IDLE_STATE);
    }

    private void PlayOffState() 
    {
        animator.Play(OFF_STATE);
    }
    IEnumerator OpenInteractionUi() 
    {
        isOpening = true;
        PlayOnState();
        yield return new WaitForSeconds(ON_OFF_ANIM_TIME);
        PlayIdleState();
        isOpening = false;
    }
    IEnumerator CloseInteractionUi()
    {
        PlayOffState();
        yield return new WaitForSeconds(ON_OFF_ANIM_TIME);
        if(!isOpening) 
        {
            gameObject.SetActive(false);
        }
    }

    public override void Init()
    {
        

    }
}
