using System.Collections;using System.Collections.Generic;
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

    private Vector3 moveVec;
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

    [Header("Player Step Climb")]
    [SerializeField]
    private float stepHeight = 1.0f;
    [SerializeField]
    private float stepSmooth = 7f;
    private int stairLayer;
    private bool isStepClimb;

    private int hp;
    public int Hp { get => hp; }


    private void Awake()
    {
        playerStateController = new PlayerStateController(this);
        interactionDetecter.Init();
        glitchEffectController = Managers.UI.MakeSceneUI<DebugModGlitchEffectController>(null,"GlitchEffect");
        hpController = Managers.UI.MakeSceneUI<HpController>(null, "HpUI");
        groundLayer = (1 << LayerMask.NameToLayer("Ground"));
        stairLayer = 1 << LayerMask.NameToLayer("Stair");
        hp = 4;
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
        hpController.Close();
    }

    #endregion
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
        var moveVec = new Vector3(hor,0,ver).normalized;
        moveVec = Quaternion.Euler(new Vector3(0,rotater.rotation.eulerAngles.y,0)) * moveVec;
        var speed = moveSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.PLAYER);
        var nextPos = pos + moveVec * speed * Managers.Time.GetDeltaTime(TIME_TYPE.PLAYER) + (moveVec.normalized * box.size.x / 2);

        var checkVecOne = Quaternion.Euler(new Vector3(0,90.0f,0)) * moveVec * 0.5f;
        var checkVecTwo = Quaternion.Euler(new Vector3(0,-90.0f,0)) * moveVec * 0.5f;
        var checkOne = nextPos + checkVecOne;
        var checkTwo = nextPos + checkVecTwo;

        Debug.DrawRay(checkOne,Vector3.down * (box.size.y * 0.5f + stepHeight),Color.red);
        Debug.DrawRay(checkTwo,Vector3.down * (box.size.y * 0.5f + stepHeight),Color.red);
        if(Physics.Raycast(checkOne,Vector3.down,box.size.y * 0.5f + stepHeight) && Physics.Raycast(checkTwo,Vector3.down,box.size.y * 0.5f + stepHeight))
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

        isOnSlope = IsOnSlope();
       
        if(new Vector3(hor,0,ver).magnitude <= 0.1f)
        {
            SetStateIdle();
            rigid.velocity = Vector3.zero;
            return;
        }

        moveVec = new Vector3(hor,0,ver);

      
        if(hor != 0) 
        {
            var dir = hor < 0 ? 1 : -1;
            skeletonAnimation.skeleton.ScaleX = dir;
        }

        moveVec = Quaternion.Euler(new Vector3(0, rotater.rotation.eulerAngles.y, 0)) * moveVec.normalized;
        Vector3 gravity = Vector3.down * Mathf.Abs(rigid.velocity.y);

        var pos = transform.position;
        var speed = moveSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.PLAYER);
        var nextPos = pos + moveVec * speed * Managers.Time.GetDeltaTime(TIME_TYPE.PLAYER) + (moveVec.normalized * box.size.x/2);
        
        if(StepClimb())
        {
            return;
        }

        if (isOnSlope)
        {
            moveVec = AdjustDirectionToSlope(moveVec);
            gravity = Vector3.zero;
            rigid.useGravity = false;
        }
        else
        {
            rigid.useGravity = true;
        }

        var checkVecOne = Quaternion.Euler(new Vector3(0, 90.0f, 0)) * moveVec * 0.5f;
        var checkVecTwo = Quaternion.Euler(new Vector3(0, -90.0f, 0)) * moveVec * 0.5f;
        var checkOne = nextPos + checkVecOne;
        var checkTwo = nextPos + checkVecTwo;

        Debug.DrawRay(checkOne,Vector3.down * (box.size.y * 0.5f + stepHeight),Color.red);
        Debug.DrawRay(checkTwo,Vector3.down * (box.size.y * 0.5f + stepHeight),Color.red);
        if ((Physics.Raycast(checkOne, Vector3.down, box.size.y * 0.5f + stepHeight) && Physics.Raycast(checkTwo, Vector3.down, box.size.y * 0.5f + stepHeight)))
        {
            var moveVelocity = moveVec * speed;
            rigid.velocity = moveVelocity + gravity;
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
        
        if(hor != 0)
        {
            var dir = hor < 0 ? 1 : -1;
            skeletonAnimation.skeleton.ScaleX = dir;
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
        Vector3 boxVec = new Vector3(box.size.x, 0, 0) * 0.5f;
        boxVec = Quaternion.Euler(new Vector3(0, rotater.rotation.eulerAngles.y + 90f, 0)) * boxVec;
        Ray ray = new Ray(transform.position + boxVec, Vector3.down);
        Debug.DrawRay(transform.position + boxVec, Vector3.down * transform.position.y, Color.blue);
        if(Physics.Raycast(ray, out slopeHit, transform.position.y, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if(angle != 0 &&angle < maxSlopeAngle) 
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
    public bool StepClimb()
    {
        var position = Quaternion.Euler(0, rotater.rotation.eulerAngles.y + 90.0f, 0) * new Vector3(box.size.x * -0.5f, box.size.y * 0.5f, 0);
       // Debug.DrawRay(transform.position - position, moveVec * 1.5f, Color.green);
        if(Physics.Raycast(transform.position - position, moveVec, 1.5f, stairLayer))
        {
         //   Debug.DrawRay(transform.position - new Vector3(box.size.x * -0.5f, box.size.y * 0.5f - stepHeight, 0), moveVec * 1.6f, Color.green);
            if(!Physics.Raycast(transform.position - new Vector3(box.size.x * -0.5f, box.size.y * 0.5f - stepHeight, 0), moveVec, 1.6f, stairLayer))
            {
                rigid.useGravity = false;
                rigid.position -= new Vector3(0f, -stepSmooth * Time.fixedDeltaTime, 0f);
                return true;
            }
            else
            {
                return false;
            } 
        }
        else
        {
            return false;
        }
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
