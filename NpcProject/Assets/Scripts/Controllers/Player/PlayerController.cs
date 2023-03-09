using System.Collections;using System.Collections.Generic;
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
    private Rigidbody rigid;
 
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

    private PlayerAnimationController.AnimDir curDir = PlayerAnimationController.AnimDir.Front;
    private DebugModGlitchEffectController glitchEffectController;
    private PlayerStateController playerStateController;
    private PlayerUIController playerUIController;
    private DeathUIController deathUIController;

    private RaycastHit slopeHit;
    private int groundLayer;
    public int Hp { get => hp; }
    public int MaxHp { get => maxHp; }

    private readonly float CHECK_RAY_WIDTH = 0.3f;
    private readonly float WIRE_EFFECT_OPEN_TIME = 3f;
    private readonly float WIRE_EFFECT_CLOSE_TIME = 1f;
    private void Awake()
    {
        playerStateController = new PlayerStateController(this);
        interactionDetecter.Init();
        hp = maxHp;
        glitchEffectController = Managers.UI.MakeSceneUI<DebugModGlitchEffectController>(null,"GlitchEffect");
        groundLayer = (1 << LayerMask.NameToLayer("Ground"));
        playerUIController = Managers.UI.MakeSceneUI<PlayerUIController>(null, "PlayerUI");
        deathUIController = Managers.UI.MakeSceneUI<DeathUIController>(null, "DeathUI");
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


    #region OnStateExit
    public void InteractionExit()
    {
        interactionDetecter.InteractionUiEnable();
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
    }
    public void ExitDebugMod()
    {
        SetstateStop();
        glitchEffectController.ExitDebugMod(() => {
            interactionDetecter.SwitchDebugMod(false);
            SetStateIdle();
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
        Debug.DrawRay(transform.position, Vector3.down * transform.position.y, Color.blue);  
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, transform.position.y , groundLayer))
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

    public void GetDamage(int damage)
    {
        hp = hp - damage;
        playerUIController.SetHp();
        if(hp == 0)
        {
            SetstateDeath();
        }
    }

    public void OpenPlayerUI()
    {
        playerUIController.OnPlayerUI();
    }
    public void ClosePlayerUI()
    {
        playerUIController.OffPlayerUI();
    }
    public void isDebugButton()
    {
        playerUIController.DebugButtom();
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
    #region WireEffect
    public void OpenWireEffect(Vector3 size, Material[] materials)
    {
        wireframeMaskController.materials = materials;
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

}
