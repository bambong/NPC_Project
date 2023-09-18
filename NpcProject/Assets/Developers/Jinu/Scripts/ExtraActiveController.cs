using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraActiveController : MonoBehaviour
{
    [SerializeField]
    private int progress;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Define.Scene transitionScene;
    [SerializeField]
    private string defalutAnim;

    public void Start()
    {
        if(progress == Managers.Data.Progress)
        {   
            this.gameObject.SetActive(true);
            if (defalutAnim != "")
            {
                anim.Play(defalutAnim);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SideIdle()
    {
        anim.Play("Meria_Idle_Side");
    }

    public void PlayerLeftIdle()
    {
        StartCoroutine(ChangePlayerAnim(PlayerAnimationController.MoveDir.Right));
    }

    IEnumerator ChangePlayerAnim(PlayerAnimationController.MoveDir dir)
    {
        yield return null;
        Managers.Game.Player.AnimIdleEnter(dir);
    }

    public void LoadScene()
    {
        Managers.Game.Player.SetstateStop();
        Managers.Scene.LoadScene(transitionScene);
    }
}
