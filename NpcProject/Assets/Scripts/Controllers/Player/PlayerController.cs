using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    private PlayerStateController playerStateController;
    [SerializeField]
    private SkeletonAnimation skeletonAnimation;
    [SpineAnimation]
    [SerializeField]
    private string idleAnim;
    [SpineAnimation]
    [SerializeField]
    private string runAnim;

    private void Awake()
    {
        playerStateController = new PlayerStateController(this);
    }

    void Update()
    {
        playerStateController.Update();
    }



    #region OnStateEnter
    public void SetAnimIdle() 
    {
        skeletonAnimation.AnimationState.SetAnimation(0,idleAnim,true);
    }
    public void SetAnimRun()
    {
        skeletonAnimation.AnimationState.SetAnimation(0,runAnim,true);
    }
    #endregion

    #region OnStateUpdate
    public void PlayerMoveUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        if(Mathf.Abs(hor) <= 0.2f && Mathf.Abs(ver) <= 0.2f)
        {
            playerStateController.ChangeState(PlayerIdle.Instance);
            return;
        }

        if(hor < 0) 
        {
            skeletonAnimation.skeleton.ScaleX = 1;
        }
        else if(hor > 0)
        {
            skeletonAnimation.skeleton.ScaleX = -1;
        }

        var moveVec = new Vector3(hor,0,ver).normalized;
        var pos = transform.position;
        var speed = moveSpeed * Time.deltaTime;

        pos -= moveVec * speed;
        transform.position = pos;
    }
    public void PlayerInputCheck()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        if(hor != 0 || ver != 0)
        {
            playerStateController.ChangeState(PlayerMove.Instance);
        }
    }
    #endregion


}
