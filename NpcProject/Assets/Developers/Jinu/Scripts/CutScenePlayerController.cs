using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CutScenePlayerController : MonoBehaviour
{
    [SerializeField]
    private SkeletonAnimation sideSpineAnim;
    [SerializeField]
    private SkeletonAnimation frontIdleSpineAnim;
    [SerializeField]
    private SkeletonAnimation backIdleSpineAnim;
    [SerializeField]
    private Animator frontBackMoveframeAnim;
    [SerializeField]
    private GameObject cutScenePlayer;

    private List<GameObject> cutAnims = new List<GameObject>();

    public void Awake()
    {
        cutAnims.Add(sideSpineAnim.gameObject);
        cutAnims.Add(frontIdleSpineAnim.gameObject);
        cutAnims.Add(backIdleSpineAnim.gameObject);
        cutAnims.Add(frontBackMoveframeAnim.gameObject);
    }
    
    public void CutScenePlayerSpawn()
    {
        cutScenePlayer.transform.position = Managers.Game.Player.gameObject.transform.position;
        Managers.Game.Player.gameObject.SetActive(false);
    }

    public void PlayerSpawn()
    {
        Managers.Game.Player.gameObject.SetActive(true);
        Managers.Game.Player.gameObject.transform.position = cutScenePlayer.transform.position;
    }

    #region CutScenePlayerAnim
    public void LeftIdle()
    {
        EnableAnim(sideSpineAnim.gameObject);
        sideSpineAnim.AnimationState.SetAnimation(0, "idel", true);
        sideSpineAnim.skeleton.ScaleX = 1;
    }

    public void RightIdle()
    {
        EnableAnim(sideSpineAnim.gameObject);
        sideSpineAnim.AnimationState.SetAnimation(0, "idel", true);
        sideSpineAnim.skeleton.ScaleX = -1;
    }

    public void FrontIdle()
    {
        EnableAnim(frontIdleSpineAnim.gameObject);
    }

    public void BackIdle()
    {
        EnableAnim(backIdleSpineAnim.gameObject);
    }

    public void LeftMove()
    {
        EnableAnim(sideSpineAnim.gameObject);
        sideSpineAnim.AnimationState.SetAnimation(0, "animation", true);
        sideSpineAnim.skeleton.ScaleX = 1;
    }

    public void RightMove()
    {
        EnableAnim(sideSpineAnim.gameObject);
        sideSpineAnim.AnimationState.SetAnimation(0, "animation", true);
        sideSpineAnim.skeleton.ScaleX = -1;
    }

    public void FrontMove()
    {
        EnableAnim(frontBackMoveframeAnim.gameObject);
        frontBackMoveframeAnim.SetBool("IsFront", true);
    }

    public void BackMove()
    {
        EnableAnim(frontBackMoveframeAnim.gameObject);
        frontBackMoveframeAnim.SetBool("IsFront", false);
    }
    private void EnableAnim(GameObject target)
    {
        for (int i = 0; i < cutAnims.Count; ++i)
        {
            if (cutAnims[i] != target)
            {
                cutAnims[i].SetActive(false);
            }
        }
        target.SetActive(true);
    }
    #endregion
}
