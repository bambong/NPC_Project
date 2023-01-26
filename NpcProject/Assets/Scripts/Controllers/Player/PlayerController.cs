using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private SkeletonAnimation skeletonAnimation;
    [SpineAnimation]
    [SerializeField]
    private string idleAnim;
    [SpineAnimation]
    [SerializeField]
    private string runAnim;

    [SerializeField]
    private InteractionDetectController interactionDetecter;
    [SerializeField]
    private Transform rotater;
    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private Rigidbody rigid;

    private GlitchEffectController glitchEffectController;
    private PlayerStateController playerStateController;

    public KeywordEntity CurKeywordInteraction { get => interactionDetecter.CurKeywordIteraction; }

    private void Awake()
    {
        playerStateController = new PlayerStateController(this);
        interactionDetecter.Init();
        glitchEffectController = Managers.UI.MakeSceneUI<GlitchEffectController>(null,"GlitchEffect");
    }

    void Update()
    {
        rotater.rotation = Camera.main.transform.rotation;
        playerStateController.Update();
    }
    private void FixedUpdate()
    {
        playerStateController.FixedUpdate();
    }

    #region OnStateEnter
    public void AnimIdleEnter() 
    {
        skeletonAnimation.AnimationState.SetAnimation(0,idleAnim,true);
    }
    public void AnimRunEnter()
    {
        skeletonAnimation.AnimationState.SetAnimation(0,runAnim,true);
    }
    public void InteractionEnter() 
    {
        interactionDetecter.InteractionUiDisable();
    }

    #endregion
    #region OnStateExit
    public void InteractionExit()
    {
        interactionDetecter.InteractionUiEnable();
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
            rigid.velocity = Vector3.zero;
            return;
        }
        
        if(hor != 0) 
        {
            var dir = hor < 0 ? 1 : -1;
            skeletonAnimation.skeleton.ScaleX = dir;
        }

        var moveVec = new Vector3(hor,0,ver).normalized;
        var pos = transform.position;
        var speed = moveSpeed * Time.fixedUnscaledDeltaTime;

        moveVec = Quaternion.Euler(new Vector3(0,rotater.rotation.eulerAngles.y,0)) * moveVec;

        rigid.velocity = moveVec *speed;
       // characterController.Move( moveVec * speed);
    }
    public void PlayerInputCheck()
    {
        if(InteractionInputCheck())
        {
            return;
        }
        if (DebugModEnterInputCheck()) 
        {
            return;
        }

        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        if(hor != 0 || ver != 0)
        {
            playerStateController.ChangeState(PlayerMove.Instance);
        }
    }
    public bool InteractionInputCheck() 
    {
        if(Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.INTERACTION_KEY)))
        {
            rigid.velocity = Vector3.zero;
            interactionDetecter.Interaction();
            return true;
        }
        return false;
    }
    public bool DebugModEnterInputCheck() 
    {
        if (Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.DEBUGMOD_KEY)))
        {
            if (Managers.Game.IsDebugMod) 
            {
                Debug.Log("SetState Normal");
                glitchEffectController.OffGlitch();
                Managers.Game.SetStateNormal();
                interactionDetecter.SwitchDebugMod(false);
            }
            else 
            {
                Debug.Log("SetState Debug");
                glitchEffectController.OnGlitch();
                Managers.Game.SetStateDebugMod();
                interactionDetecter.SwitchDebugMod(true);
            }
            return true;
        }
        return false;
    }
    public void KeywordModInputCheck()
    {
        if(Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.EXIT_KEY)))
        {
            Managers.Keyword.ExitKeywordMod();
        }

    }
    #endregion

    #region SetState
    public void SetStateInteraction() 
    {
        playerStateController.ChangeState(PlayerInteraction.Instance);
    }
    public void SetStateIdle() 
    {
        playerStateController.ChangeState(PlayerIdle.Instance);
    }
    public void SetStatekeywordMod()
    {
        playerStateController.ChangeState(PlayerKeywordMod.Instance);
    }

    #endregion

}
