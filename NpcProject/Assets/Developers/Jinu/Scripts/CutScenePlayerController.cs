using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.Playables;

public class CutScenePlayerController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;    
    [SerializeField]
    private Animator cutSceneAnimatorController;
    [SerializeField]
    private GameObject cutScenePlayer;
    [SerializeField]
    private PlayableDirector curPlayable;
    [SerializeField]
    private Define.Scene transitionScene;
    [SerializeField]
    private Define.SOUND transitionSound;

    private Transform startpoint;
    private bool isleft;    


    public void Awake()
    {
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;        
    }

    public void SetStartPoint(Transform transform, bool left)
    {
        startpoint = transform;
        isleft = left;
    }

    #region Signal
    public void CutScenePlayerSpawn()
    {
        cutScenePlayer.transform.position = Managers.Game.Player.gameObject.transform.position;
        Managers.Game.Player.gameObject.SetActive(false);
    }

    public void PlayerMove()
    {
        curPlayable.Pause();
        CheckAnimation();
        StartCoroutine(MovePoint());
    }

    private void CheckAnimation()
    {
        if ((cutScenePlayer.transform.position - startpoint.transform.position).x > 0)
        {
            LeftWalk();
        }
        else
        {
            RightWalk();
        }
    }

    public void SignalSceneLoad()
    {
        Managers.Sound.PlaySFX(transitionSound);
        Managers.Scene.LoadScene(transitionScene);
    }

    IEnumerator MovePoint()
    {        
        while (cutScenePlayer.transform.position != startpoint.transform.position)
        {   
            cutScenePlayer.transform.position = Vector3.MoveTowards(cutScenePlayer.transform.position, startpoint.transform.position, 5f * Time.deltaTime);
            yield return null;
        }
        if(isleft)
        {
            LeftIdle();
        }
        else
        {
            RightIdle();
        }
        curPlayable.Resume();
    }

    public void PlayerSpawn()
    {
        Managers.Game.Player.gameObject.SetActive(true);
        Managers.Game.Player.gameObject.transform.position = cutScenePlayer.transform.position;
    }
    #endregion

    #region CutScenePlayerAnim
    public void LeftIdle()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        spriteRenderer.transform.localScale = scale;

        cutSceneAnimatorController.Play("PlayerSide_Idle");
    }

    public void RightIdle()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -1;
        spriteRenderer.transform.localScale = scale;

        cutSceneAnimatorController.Play("PlayerSide_Idle");
    }

    public void FrontIdle()
    {
        cutSceneAnimatorController.Play("PlayerFront_Idle");
    }

    public void BackIdle()
    {
        cutSceneAnimatorController.Play("PlayerBack_Idle");
    }

    public void LeftWalk()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        spriteRenderer.transform.localScale = scale;

        cutSceneAnimatorController.Play("PlayerSide_Walk");
    }

    public void RightWalk()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -1;
        spriteRenderer.transform.localScale = scale;

        cutSceneAnimatorController.Play("PlayerSide_Walk");

    }

    public void FrontWalk()
    {
        cutSceneAnimatorController.Play("PlayerFront_Walk");

    }

    public void BackWalk()
    {
        cutSceneAnimatorController.Play("PlayerBack_Walk");
    }

    public void LeftRun()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        spriteRenderer.transform.localScale = scale;

        cutSceneAnimatorController.Play("PlayerSide_Run");
    }

    public void RightRun()
    {
        var scale = spriteRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -1;
        spriteRenderer.transform.localScale = scale;

        cutSceneAnimatorController.Play("PlayerSide_Run");
    }

    public void FrontRun()
    {
        cutSceneAnimatorController.Play("PlayerFront_Run");

    }

    public void BackRun()
    {
        cutSceneAnimatorController.Play("PlayerBack_Run");
    }

    #endregion
}
