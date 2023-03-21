using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using UnityEditor.ShaderGraph.Internal;
using static UnityEditor.PlayerSettings;
using UnityEditor.Animations;
using UnityEditor.Rendering.Utilities;
using System;
using DG.Tweening;
using AmazingAssets.WireframeShader;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;
using MoreMountains.Feedbacks;
using Spine.Unity.Examples;

public class PlayerController : MonoBehaviour
{
    [Header("Player Element")]
    [Space(1)]
    [SerializeField]
    private PlayerAnimationController animationController;
    [SerializeField]
    private InteractionDetectController interactionDetecter;
    [SerializeField]
    private Transform rotater;
    [SerializeField]
    private BoxCollider box;
    [SerializeField]
    private CapsuleCollider col;

    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private ConstantForce gravityForce;


    [Space(1)]
    [Header("Player Move Option")]
    [Space(1)]
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private int maxSlopeAngle = 40;
    [SerializeField]
    private float moveEnableDis = 0.5f;
    [SerializeField]
    private float stepHeight = 1.0f;

    [Space(1)]
    [Header("Player HP")]
    [Space(1)]
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int hp;

    [Space(1)]
    [Header("WireEffect")]
    [SerializeField]
    private GameObject wireEffectGo;
    [SerializeField]
    private WireframeMaskController wireframeMaskController;

    [Space(1)]
    [Header("MM_Feedback")]
    [SerializeField]
    private MMFeedbacks deathFeedback;

    [Space(1)]
    [Header("Motion Effec")]
    [SerializeField]
    private SkeletonGhost ghost;
    [SerializeField]
    private SpineGhostColorController motionTrail;

    private PlayerAnimationController.AnimDir curDir = PlayerAnimationController.AnimDir.Front;
    private DebugModGlitchEffectController glitchEffectController;
    private PlayerStateController playerStateController;
    private PlayerUIController playerUIController;
    private DeathUIController deathUIController;

    private RaycastHit slopeHit;
    private int groundLayer;
    public bool IsDebugMod { get => isDebugMod; }
    private bool isDebugMod;
    public int Hp { get => hp; }
    public int MaxHp { get => maxHp; }

    private readonly float CHECK_RAY_WIDTH = 0.3f;
    private readonly float WIRE_EFFECT_OPEN_TIME = 2f;
    private readonly float WIRE_EFFECT_CLOSE_TIME = 1f;
    private readonly float PLAYER_ANIM_COS = 0.71f;
    private readonly float PLAYER_DIR_WEIGHT = 0.1f;
    private void Awake()
    {
        playerStateController = new PlayerStateController(this);
        interactionDetecter.Init();
        hp = maxHp;
        glitchEffectController = Managers.UI.MakeSceneUI<DebugModGlitchEffectController>(null, "GlitchEffect");
        groundLayer = (1 << LayerMask.NameToLayer("Slope"));
        playerUIController = Managers.UI.MakeWorldSpaceUI<PlayerUIController>(transform, "PlayerUI");
        deathUIController = Managers.UI.MakeCameraSpaceUI<DeathUIController>(1f,null, "DeathUI");
      
    }

    void Update()
    {
        var rot = Camera.main.transform.rotation.eulerAngles;
        rot.x = 0;
        rotater.rotation = Quaternion.Euler(rot);
        playerStateController.Update();
        if (transform.lossyScale != Vector3.one)
        {
            var factor = 1 / transform.lossyScale.x;
            transform.localScale *= factor;
        }

    }
    private void FixedUpdate()
    {
        playerStateController.FixedUpdate();

    }

    public void InteractionEnter()
    {
        interactionDetecter.InteractionUiDisable();
    }

    public void SetMotionEffect(bool isOn) 
    {
        ghost.enabled = isOn;
        motionTrail.enabled = isOn;
    }
    #region OnStateExit
    public void InteractionExit()
    {
        interactionDetecter.InteractionUiEnable();
    }

    #endregion
    #region OnStateUpdate
    public bool IsMove(Vector3 pos, float hor, float ver)
    {

        var moveVec = new Vector3(hor, 0, ver).normalized;
        moveVec =rotater.transform.TransformDirection(moveVec);

        var boxHalfSize = box.size.x * 0.5f;
        var checkWidth = box.size.x * CHECK_RAY_WIDTH;
        
        var isSlope = IsOnSlope();
        moveVec = MoveRayCheck(moveVec, isSlope);
        if (isSlope)
        {
            moveVec = AdjustDirectionToSlope(moveVec);

        }


        if (new Vector3(moveVec.x, 0, moveVec.z).magnitude > 0.02f)
        {
            return true;
        }
        else
        {
            return false;
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
        moveVec = rotater.transform.TransformDirection(moveVec);
        Vector3 gravity = Vector3.down * Mathf.Abs(rigid.velocity.y);

        var pos = transform.position;
        var speed = moveSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.PLAYER);

        var isSlope = IsOnSlope();
        var preDir = curDir;
        CurrentAnimDirUpdtae(moveVec);
        moveVec = MoveRayCheck(moveVec, isSlope);
       
        if (isSlope)
        {
            moveVec = AdjustDirectionToSlope(moveVec);
            gravity = Vector3.zero;
            rigid.useGravity = false;
            gravityForce.enabled = false;
            Debug.DrawRay(pos, moveVec * 10, Color.blue);
        }
        else
        {
            rigid.useGravity = true;
            gravityForce.enabled = true;
        }

        if (new Vector3(moveVec.x, 0, moveVec.z).magnitude > 0.02f)
        {
            AnimMoveEnter();
            rigid.velocity = moveVec.normalized * speed + gravity;  
        }
        else
        {
            curDir = preDir;
            rigid.velocity = Vector3.zero;
            SetStateIdle();
        }
    }

    private Vector3 MoveRayCheck(Vector3 moveVec, bool isSlope )
    {
        var pos = transform.position + (Vector3.down* box.size.y * 0.5f) + (Vector3.down*stepHeight* 0.5f);
        var boxHalfSize = box.size.x * 0.5f;
        var checkWidth = box.size.x * CHECK_RAY_WIDTH;
        float moveEnableWidth = box.size.x * 0.25f;
        float boxHeight = (stepHeight / 2);
        int layer = (-1) - (1 << LayerMask.NameToLayer("Player"));
        if (Mathf.Abs(moveVec.x) > 0)
        {
            Vector3 xPos = pos;
            if (isSlope) 
            {
                xPos.y += AdjustDirectionToSlope(new Vector3(moveVec.x,0,0)).y;
            }
            var dir = (moveVec.x > 0 ? 1 : -1) * (boxHalfSize + moveEnableDis/2) * Vector3.right;
            var boxSize = new Vector3(moveEnableDis / 2, boxHeight, moveEnableWidth);
            ExtDebug.DrawBox(xPos + dir + new Vector3(0, 0, checkWidth),boxSize,Quaternion.identity, Color.red);
            ExtDebug.DrawBox(xPos + dir - new Vector3(0, 0, checkWidth), boxSize, Quaternion.identity, Color.red);
    
            if (!Physics.CheckBox(xPos + dir - new Vector3(0, 0, checkWidth),boxSize,Quaternion.identity, layer, QueryTriggerInteraction.Ignore))
            {
                moveVec.x = 0;
            }
            else if (!Physics.CheckBox(xPos + dir + new Vector3(0, 0, checkWidth), boxSize, Quaternion.identity, layer, QueryTriggerInteraction.Ignore))
            {
                moveVec.x = 0;
            }
        }
        if (Mathf.Abs(moveVec.z) > 0)
        {
            Vector3 zPos = pos;
            if (isSlope)
            {
                zPos.y += AdjustDirectionToSlope(new Vector3(0, 0, moveVec.z)).y;
            }

            var dir = (moveVec.z > 0 ? 1 : -1) * (boxHalfSize + moveEnableDis / 2) * Vector3.forward ;
            var boxSize = new Vector3(moveEnableWidth, boxHeight, moveEnableDis / 2);
            ExtDebug.DrawBox(zPos + dir + new Vector3(checkWidth, 0, 0), boxSize, Quaternion.identity, Color.red);
            ExtDebug.DrawBox(zPos + dir - new Vector3(checkWidth, 0, 0), boxSize, Quaternion.identity, Color.red);
            if (!Physics.CheckBox(zPos + dir + new Vector3(checkWidth, 0, 0), boxSize, Quaternion.identity, layer ,QueryTriggerInteraction.Ignore))
            {
                moveVec.z = 0;
            }
            else if (!Physics.CheckBox(zPos + dir - new Vector3(checkWidth, 0, 0), boxSize, Quaternion.identity, layer, QueryTriggerInteraction.Ignore))
            {
                moveVec.z = 0;
            }
        }
        return moveVec;

    }
    private void CurrentAnimDirUpdtae(Vector3 moveVec) 
    {
        var moveDotVer = Vector3.Dot(rotater.transform.forward.normalized, moveVec.normalized);
        var factor = PLAYER_ANIM_COS;
        if(curDir == PlayerAnimationController.AnimDir.Front || curDir == PlayerAnimationController.AnimDir.Back) 
        {
            factor -= PLAYER_DIR_WEIGHT;
        }
        else 
        {
            factor += PLAYER_DIR_WEIGHT;
        }

        if (Mathf.Abs(moveDotVer) > factor)
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
        else if (Mathf.Abs(moveDotVer) < factor)
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

    public void PlayerInputCheck()
    {
        if (InteractionInputCheck())
        {
            return;
        }

        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        
        if(hor == 0 && ver == 0)
        {
            return;
        }
        
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
        if (!Managers.Keyword.IsDebugZoneIn || glitchEffectController.IsPlaying)
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
        isDebugMod = true;
        Managers.Game.SetStateDebugMod();
        glitchEffectController.EnterDebugMod(() =>
        {
            SetStateIdle();
            SetMotionEffect(true);
        });
        interactionDetecter.SwitchDebugMod(true);
    }
    public void ExitDebugMod()
    {
        SetstateStop();
        glitchEffectController.ExitDebugMod(() => {
            interactionDetecter.SwitchDebugMod(false);
            SetStateIdle();
            isDebugMod = false;
            isDebugButton();
            SetMotionEffect(false);
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


    public bool IsOnSlope()
    {
        Debug.DrawRay(transform.position, Vector3.down * transform.position.y, Color.blue);
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, transform.position.y, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle < maxSlopeAngle)
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

    public void GetDamage(int damage)
    {
        hp = hp - damage;
        playerUIController.SetHp(damage);
        if (hp <= 0)
        {
            SetstateDeath();
        }
    }

    public void isDebugButton()
    {
        playerUIController.DebugButton();
    }
    public void OpenDeathUI()
    {
        deathUIController.DeathUIOpen();
    }
    public void CloseDeathUI()
    {
        deathUIController.DeathUIClose();
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
    #region WireEffect
    public void SetWireframeMaterial(Material[] materials) 
    {
        wireframeMaskController.materials = materials;
    }
    public void OpenWireEffect(Vector3 size)
    {
        wireEffectGo.transform.localScale = Vector3.zero;
        wireEffectGo.transform.DOKill();
        wireEffectGo.transform.DOScale(size, WIRE_EFFECT_OPEN_TIME);
    }
    public void CloseWireEffect()
    {
        wireEffectGo.transform.DOKill();
        wireEffectGo.transform.DOScale(Vector3.zero, WIRE_EFFECT_CLOSE_TIME).OnComplete(() => wireframeMaskController.materials = null);
    }

    #endregion
    #region MM_FeedBack
    public void PlayDeathFeedback()
    {
        //rotater.gameObject.SetActive(false);
        deathFeedback.transform.SetParent(null);
        deathFeedback.PlayFeedbacks();
        rigid.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
    #endregion

}
