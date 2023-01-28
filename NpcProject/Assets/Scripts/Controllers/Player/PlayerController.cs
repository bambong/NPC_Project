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

    private DebugModGlitchEffectController glitchEffectController;
    private PlayerStateController playerStateController;

    private void Awake()
    {
        playerStateController = new PlayerStateController(this);
        interactionDetecter.Init();
        glitchEffectController = Managers.UI.MakeSceneUI<DebugModGlitchEffectController>(null,"GlitchEffect");
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
            SetStateIdle();
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
        if(!Managers.Keyword.IsDebugZoneIn || glitchEffectController.IsPlaying) 
        {
            return false;
        }

        if (Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.DEBUGMOD_KEY)))
        {
            if (Managers.Game.IsDebugMod) 
            {
                ExitDebugMod();
            }
            else 
            {
                EnterDebugMod();
            }
            return true;
        }
        return false;
    }

    public void EnterDebugMod()
    {
        SetstateStop();
        Managers.Game.SetStateDebugMod();
        glitchEffectController.EnterDebugMod(() => 
        {
            SetStateDebugMod();
        });
        interactionDetecter.SwitchDebugMod(true);
    }
    public void ExitDebugMod()
    {
        SetstateStop();
        glitchEffectController.ExitDebugMod(() => {
            interactionDetecter.SwitchDebugMod(false);
            SetStateIdle();
        });
    }
    public void ClearMoveAnim()
    {
        AnimIdleEnter();
        rigid.velocity = Vector3.zero;
    }
    public void KeywordModInputCheck()
    {
        if(Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.EXIT_KEY)))
        {
            Managers.Keyword.ExitKeywordMod();
        }
    }
    public void DebugModeMouseInputCheck()
    {
        if(!Managers.Game.IsDebugMod)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer = LayerMask.GetMask("Interaction");
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,float.MaxValue,layer))
            {
                var entity = hit.collider.GetComponent<KeywordEntity>();

                if(entity == null)
                {
                    return;
                }
                Managers.Keyword.EnterKeywordMod(entity);
            }
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
    public void SetStateDebugMod()
    {
        playerStateController.ChangeState(PlayerDebugMod.Instance);
    }
    public void SetstateStop() 
    {
        playerStateController.ChangeState(PlayerStop.Instance);
    }
    #endregion

}
