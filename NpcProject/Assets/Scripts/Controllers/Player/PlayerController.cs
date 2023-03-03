using System.Collections;using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using UnityEditor.ShaderGraph.Internal;
using static UnityEditor.PlayerSettings;
using UnityEditor.Animations;
using UnityEditor.Rendering.Utilities;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private PlayerAnimationController animationController;
    [SerializeField]
    private InteractionDetectController interactionDetecter;
    [SerializeField]
    private Transform rotater;

    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private float moveEnableDis = 0.5f;
    private DebugModGlitchEffectController glitchEffectController;
    private PlayerStateController playerStateController;
    private HpController hpController;

    [Header("Player Slope Check")]
    [SerializeField]
    private int maxSlopeAngle = 40;
    [SerializeField]
    private BoxCollider box;
    private bool isOnSlope;
    private RaycastHit slopeHit;
    private int groundLayer;

    [SerializeField]
    private float stepHeight = 1.0f;

    private PlayerAnimationController.AnimDir curDir = PlayerAnimationController.AnimDir.Front;
    private int hp;
    public int Hp { get => hp; }

    private readonly float CHECK_RAY_WIDTH = 0.3f;

    private void Awake()
    {
        playerStateController = new PlayerStateController(this);
        interactionDetecter.Init();
        glitchEffectController = Managers.UI.MakeSceneUI<DebugModGlitchEffectController>(null,"GlitchEffect");
        hpController = Managers.UI.MakeSceneUI<HpController>(null, "HpUI");
        groundLayer = (1 << LayerMask.NameToLayer("Ground"));
        hp = 4;
    }

    void Update()
    {
        var rot = Camera.main.transform.rotation.eulerAngles;
        rot.x = 0;
        rotater.rotation = Quaternion.Euler(rot);
        playerStateController.Update();

    }
    private void FixedUpdate()
    {
        playerStateController.FixedUpdate();
    }
 
    public void InteractionEnter() 
    {
        interactionDetecter.InteractionUiDisable();
        hpController.Close();
    }


    #region OnStateExit
    public void InteractionExit()
    {
        interactionDetecter.InteractionUiEnable();
        hpController.Open();
    }

    #endregion
    #region OnStateUpdate
    public bool IsMove(Vector3 pos,float hor,float ver)
    {
   
        var moveVec = new Vector3(hor, 0, ver).normalized;
        //var forward = Camera.main.transform.forward;
        //forward.y = 0;
        //var angle = -1 * Vector3.Angle(Vector3.forward, forward);
        moveVec =rotater.transform.TransformDirection(moveVec);
        var boxHalfSize = box.size.x * 0.5f;
        var checkWidth = box.size.x * CHECK_RAY_WIDTH;
        var isSlope = IsOnSlope();
        if (isSlope)
        {
            moveVec = AdjustDirectionToSlope(moveVec);

        }
        moveVec = MoveRayCheck(moveVec, isSlope);


        if (new Vector3(moveVec.x, 0, moveVec.z).magnitude > 0.02f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private Vector3 MoveRayCheck(Vector3 moveVec, bool isSlope )
    {
        var pos = transform.position + (Vector3.down* box.size.y * 0.5f) + (Vector3.down*stepHeight* 0.5f);
        var boxHalfSize = box.size.x * 0.5f;
        var checkWidth = box.size.x * CHECK_RAY_WIDTH;
        float moveEnableWidth = box.size.x * 0.25f;
        float boxHeight = (stepHeight/2) + (isSlope? stepHeight/2:0);
        int layer = (-1) - (1 << LayerMask.NameToLayer("Player"));
        if (Mathf.Abs(moveVec.x) > 0)
        {
            var dir = (moveVec.x > 0 ? 1 : -1) * (boxHalfSize + moveEnableDis/2) * Vector3.right;
            var boxSize = new Vector3(moveEnableDis / 2, boxHeight, moveEnableWidth);
            ExtDebug.DrawBox(pos + dir + new Vector3(0, 0, checkWidth),boxSize,Quaternion.identity, Color.red);
            ExtDebug.DrawBox(pos + dir - new Vector3(0, 0, checkWidth), boxSize, Quaternion.identity, Color.red);
            if (!Physics.CheckBox(pos + dir - new Vector3(0, 0, checkWidth),boxSize,Quaternion.identity, layer, QueryTriggerInteraction.Ignore))
            {
                moveVec.x = 0;
            }
            else if (!Physics.CheckBox(pos + dir + new Vector3(0, 0, checkWidth), boxSize, Quaternion.identity, layer, QueryTriggerInteraction.Ignore))
            {
                moveVec.x = 0;
            }
        }
        if (Mathf.Abs(moveVec.z) > 0)
        {
            var dir = (moveVec.z > 0 ? 1 : -1) * (boxHalfSize + moveEnableDis / 2) * Vector3.forward ;
            var boxSize = new Vector3(moveEnableWidth, boxHeight, moveEnableDis / 2);
            ExtDebug.DrawBox(pos + dir + new Vector3(checkWidth, 0, 0), boxSize, Quaternion.identity, Color.red);
            ExtDebug.DrawBox(pos + dir - new Vector3(checkWidth, 0, 0), boxSize, Quaternion.identity, Color.red);
            if (!Physics.CheckBox(pos + dir + new Vector3(checkWidth, 0, 0), boxSize, Quaternion.identity, layer ,QueryTriggerInteraction.Ignore))
            {
                moveVec.z = 0;
            }
            else if (!Physics.CheckBox(pos + dir - new Vector3(checkWidth, 0, 0), boxSize, Quaternion.identity, layer, QueryTriggerInteraction.Ignore))
            {
                moveVec.z = 0;
            }
        }
        return moveVec;

    }
    private void CurrentAnimDirUpdtae(Vector3 moveVec) 
    {
        var moveDotVer = Vector3.Dot(rotater.transform.forward.normalized, moveVec.normalized);
        if (Mathf.Abs(moveDotVer) > 0.71f)
        {
            if (moveDotVer < 0)
            {
                curDir = PlayerAnimationController.AnimDir.Front;
            }
            else
            {
                curDir = PlayerAnimationController.AnimDir.Back;
            }
        }
        else
        {
            var moveDotHor = Vector3.Dot(rotater.transform.right.normalized, moveVec.normalized);
            if (moveDotHor < 0)
            {
                curDir = PlayerAnimationController.AnimDir.Left;
            }
            else
            {
                curDir = PlayerAnimationController.AnimDir.Right;
            }
        }
    }
    public void PlayerMoveUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");


        if (new Vector3(hor, 0, ver).magnitude <= 0.1f)
        {
            SetStateIdle();
            rigid.velocity = Vector3.zero;
            return;
        }

        var moveVec = new Vector3(hor, 0, ver).normalized;

        //var forward = Camera.main.transform.forward;
        //forward.y = 0;
        //var angle = -1*Vector3.Angle(Vector3.forward, forward);
        moveVec = rotater.transform.TransformDirection(moveVec);
        Vector3 gravity = Vector3.down * Mathf.Abs(rigid.velocity.y);

        var pos = transform.position;
        var speed = moveSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.PLAYER);


        var isSlope = IsOnSlope();
        if (isSlope)
        {
            moveVec = AdjustDirectionToSlope(moveVec);
            gravity = Vector3.zero;
            rigid.useGravity = false;
            Debug.DrawRay(pos, moveVec * 10, Color.blue);
        }
        else
        {
           rigid.useGravity = true;
        }

       
        //var rayPos = pos + Vector3.down * box.size.y * 0.5f;

        moveVec = MoveRayCheck(moveVec, isSlope);

     

        if (new Vector3(moveVec.x,0,moveVec.z).magnitude > 0.02f)
        {
            CurrentAnimDirUpdtae(moveVec);
            animationController.SetMoveAnim(curDir);
            rigid.velocity = moveVec.normalized * speed + gravity;
        }
        else
        {
            rigid.velocity = Vector3.zero;
            SetStateIdle();
        }
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
        

        if(hor == 0 && ver == 0)
        {
            return;
        }
        
        //if(hor != 0)
        //{
        //    var dir = hor < 0 ? 1 : -1;
        //    skeletonAnimation.skeleton.ScaleX = dir;
        //}

        if(IsMove(transform.position,hor,ver))
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
        hpController.Close();
    }
    public void ExitDebugMod()
    {
        SetstateStop();
        glitchEffectController.ExitDebugMod(() => {
            interactionDetecter.SwitchDebugMod(false);
            SetStateIdle();
            hpController.Open();
        });
    }
    public void AnimIdleEnter()
    {
        animationController.SetIdleAnim(curDir);
    }
    public void AnimMoveEnter()
    {
        animationController.SetMoveAnim(curDir);
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

    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position  , Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * transform.position.y, Color.blue);
        if(Physics.Raycast(ray, out slopeHit, transform.position.y, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if(angle < maxSlopeAngle) 
            {
                return true;   
            }
            return false;
        }
        return false;
    }
    public Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
    public void GetDamage()
    {
        if(hp > 0)
        {
            hp = hp - 1;
            hpController.GetDamage();
        }
    }
    public void GetHp()
    {
        if(hp < 4)
        {
            hp = hp + 1;
            hpController.GetHp();
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
    public void SetstateDeath()
    {
        playerStateController.ChangeState(PlayerDeath.Instance);
    }
    #endregion

}
