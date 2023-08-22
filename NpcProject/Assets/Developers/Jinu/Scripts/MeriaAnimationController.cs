using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeriaAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animatorController;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void Idle()
    {
        animatorController.Play("Meria_Idle");
    }
    public void LeftIdle()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        spriteRenderer.transform.localScale = scale;

        animatorController.Play("Meria_Idle_Side");
    }

    public void RightIdle()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -1;
        spriteRenderer.transform.localScale = scale;

        animatorController.Play("Meria_Idle_Side");
    }

    public void LeftWalk()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        spriteRenderer.transform.localScale = scale;

        animatorController.Play("Meria_Walk_Side");
    }

    public void RightWalk()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        spriteRenderer.transform.localScale = scale;

        animatorController.Play("Meria_Walk_Side");
    }

    public void FrontWalk()
    {
        animatorController.Play("Meria_Walk");
    }

    public void BackWalk()
    {
        animatorController.Play("Meria_Walk_Back");
    }

    public void WalkSound()
    {
        Managers.Sound.PlaySFX(Define.SOUND.WalkPlayer);
    }
}
