using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animatorController;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void LeftIdle()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        spriteRenderer.transform.localScale = scale;

        animatorController.Play("YooMinWooIdle");
    }

    public void RightIdle()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -1;
        spriteRenderer.transform.localScale = scale;

        animatorController.Play("YooMinWooIdle");
    }

    public void LeftWalk()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        spriteRenderer.transform.localScale = scale;

        animatorController.Play("YooMinWooWalkSide");
    }
    
    public void RightWalk()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        spriteRenderer.transform.localScale = scale;

        animatorController.Play("YooMinWooWalkSide");
    }

    public void FrontWalk()
    {
        animatorController.Play("YooMinWooWalkFront");
    }

    public void BackWalk()
    {
        animatorController.Play("YooMinWooWalkBack");
    }

    public void WalkSound()
    {
        Managers.Sound.PlaySFX(Define.SOUND.WalkPlayer);
    }
}
