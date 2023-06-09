using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AmazingAssets.WireframeShader;
using MoreMountains.Feedbacks;
using Spine.Unity.Examples;
using System.Linq;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

public class PlayerController : MonoBehaviour , IDataHandler
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
    private GameObject playerDeathTrigger;


    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private ConstantForce gravityForce;
    [SerializeField]
    private PlayerUIController playerUIController;


    [Space(1)]
    [Header("Player Move Option")]
    [Space(1)]
    [SerializeField]
    private float moveSpeed = 600f;
    [SerializeField]
    private float walkSpeed = 600f;
    [SerializeField]
    private float runSpeed = 1800f;
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
    private MMF_Player deathFeedback;
    [SerializeField]
    private MMF_Player hitFeedback;

    private PlayerAnimationController.MoveDir curDir = PlayerAnimationController.MoveDir.Front;
    private DebugModGlitchEffectController glitchEffectController;
    private PlayerStateController playerStateController;
    
    private DeathUIController deathUIController;
    private Coroutine debugModeInput;
    private RaycastHit slopeHit;
    private int slopeLayer;

    private KeywordController isFocusKeyword;
    private KeywordFrameBase isFocusFrame;
    private PurposePanelController purposePanel;
    private SignalPanelController signalPanel;
    public int Hp { get => hp; }
    public int MaxHp { get => maxHp; }
    public Transform PlayerAncestor { get; set; }
    public PurposePanelController PurposePanel { get => purposePanel;  }
    public SignalPanelController SignalPanel { get => signalPanel;  }

    private const float CHECK_RAY_WIDTH = 0.3f;
    private const float WIRE_EFFECT_OPEN_TIME = 2f;
    private const float WIRE_EFFECT_CLOSE_TIME = 1f;
    private const float PLAYER_ANIM_COS = 0.71f;
    private const float PLAYER_DIR_WEIGHT = 0.1f;

    private const string KEYWORD_FRAME_TAG = "KeywordFrame";
    private const string KEYWORD_TAG = "KeywordController";
    private void Awake()
    {
        playerStateController = new PlayerStateController(this);
        interactionDetecter.Init();
        hp = maxHp;
        glitchEffectController = Managers.UI.MakeSceneUI<DebugModGlitchEffectController>(null, "GlitchEffect");
        slopeLayer = (1 << LayerMask.NameToLayer("Slope"));
       // playerUIController = Managers.UI.MakeWorldSpaceUI<PlayerUIController>(transform, "PlayerUI");
        deathUIController = Managers.UI.MakeCameraSpaceUI<DeathUIController>(1f,null, "DeathUI");
        deathUIController.DeathUIClose();
    }
    private void Start()
    {
        purposePanel = Managers.UI.MakeSceneUI<PurposePanelController>(null,"PurposePanel");
        signalPanel = Managers.UI.MakeSceneUI<SignalPanelController>(null,"SignalPanel");
    }
    void Update()
    {
        var rot = Camera.main.transform.rotation.eulerAngles;
        rot.x = 0;
        rot.y += 180;
        transform.rotation = Quaternion.identity;
        rotater.rotation = Quaternion.Euler(rot);
        playerStateController.Update();
        if (transform.lossyScale != Vector3.one)
        {
            var factor = 1 / transform.lossyScale.x;
            transform.localScale *= factor;
        }

        //if (Input.GetKeyDown(KeyCode.Escape)) 
        //{
        //    Managers.Data.UpdateProgress(Managers.Data.Progress + 1);
               
        //}
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
    public bool IsMove(Vector3 pos, float hor, float ver)
    {

        var moveVec = new Vector3(hor, 0, ver).normalized;
        moveVec = rotater.transform.TransformDirection(-moveVec);

        var boxHalfSize = box.size.x * 0.5f;
        var checkWidth = box.size.x * CHECK_RAY_WIDTH;
        
        var isSlope = IsOnSlope(moveVec);
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

    public void PlayerWalkUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        var isRun = Input.GetKey(Managers.Game.Key.ReturnKey(KEY_TYPE.RUN_KEY));

        if (isRun)
        {
            SetStateRun();
            return;
        }

        if (new Vector3(hor, 0, ver).magnitude <= 0.1f)
        {
            SetStateIdle();
            rigid.velocity = Vector3.zero;
            return;
        }
        
        var moveVec = new Vector3(hor, 0, ver).normalized;
        moveVec = rotater.transform.TransformDirection(-moveVec);
        Vector3 gravity = Vector3.down * Mathf.Abs(rigid.velocity.y);

        var pos = transform.position;
        var speed = moveSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.PLAYER);

        var isSlope = IsOnSlope(moveVec);
        var preDir = curDir;
        CurrentAnimDirUpdate(moveVec);
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
            AnimMoveEnter(new Vector3(hor, 0, ver));
            rigid.velocity = moveVec.normalized * speed + gravity;
        }
        else
        {
            curDir = preDir;
            rigid.velocity = Vector3.zero;
            SetStateIdle();
        }
    }

    public void PlayerRunUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        var isRun = Input.GetKey(Managers.Game.Key.ReturnKey(KEY_TYPE.RUN_KEY));

        if(!isRun)
        {
            if (new Vector3(hor, 0, ver).magnitude <= 0.1f)
            {
                SetStateIdle();
                rigid.velocity = Vector3.zero;
                return;
            }
            SetStateWalk();
            return;
        }

        var moveVec = new Vector3(hor, 0, ver).normalized;
        moveVec = rotater.transform.TransformDirection(-moveVec);
        Vector3 gravity = Vector3.down * Mathf.Abs(rigid.velocity.y);

        var pos = transform.position;
        var speed = moveSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.PLAYER);

        var isSlope = IsOnSlope(moveVec);
        var preDir = curDir;
        CurrentAnimDirUpdate(moveVec);
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
            AnimRunEnter(new Vector3(hor, 0, ver));
            rigid.velocity = moveVec.normalized * speed + gravity;
        }
        else
        {
            curDir = preDir;
            rigid.velocity = Vector3.zero;
            SetStateIdle();
        }
    }

    public void ChangeToRunSpeed(bool run)
    {
        if (run)
            moveSpeed = runSpeed;
        else
            moveSpeed = walkSpeed;
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
            float slopeAmountY =0 ;
            if (isSlope) 
            {
                 slopeAmountY = Mathf.Abs(AdjustDirectionToSlope(new Vector3(moveVec.x,0,0)).y);
                //xPos.y += slopeAmountY/2;
            }
            var dir = (moveVec.x > 0 ? 1 : -1) * (boxHalfSize + moveEnableDis/2) * Vector3.right;
            var boxSize = new Vector3(moveEnableDis / 2, boxHeight + slopeAmountY, moveEnableWidth);
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
            //if (isSlope)
            //{
            //    zPos.y += AdjustDirectionToSlope(new Vector3(0, 0, moveVec.z)).y;
            //}
            float slopeAmountY = 0;
            if (isSlope)
            {
                slopeAmountY = Mathf.Abs(AdjustDirectionToSlope(new Vector3(0, 0, moveVec.z)).y);
                //xPos.y += slopeAmountY/2;
            }
            var dir = (moveVec.z > 0 ? 1 : -1) * (boxHalfSize + moveEnableDis / 2) * Vector3.forward ;
            var boxSize = new Vector3(moveEnableWidth, boxHeight + slopeAmountY, moveEnableDis / 2);
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
    private void CurrentAnimDirUpdate(Vector3 moveVec) 
    {
        var moveDotVer = Vector3.Dot(-rotater.transform.forward.normalized, moveVec.normalized);
        var factor = PLAYER_ANIM_COS;
        if(curDir == PlayerAnimationController.MoveDir.Front || curDir == PlayerAnimationController.MoveDir.Back) 
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
                curDir = PlayerAnimationController.MoveDir.Front;
            }
            else
            {
                curDir = PlayerAnimationController.MoveDir.Back;
            }
        }
        else if (Mathf.Abs(moveDotVer) < factor)
        {
            var moveDotHor = Vector3.Dot(rotater.transform.right.normalized, moveVec.normalized);
            if (moveDotHor < 0)
            {
                curDir = PlayerAnimationController.MoveDir.Left;
            }
            else
            {
                curDir = PlayerAnimationController.MoveDir.Right;
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
        
        if (IsOnSlope(Vector3.zero))
        {
            rigid.useGravity = false;
            gravityForce.enabled = false;
 
        }
        else
        {
            rigid.useGravity = true;
            gravityForce.enabled = true;
        }

        if (hor == 0 && ver == 0)
        {
            return;
        }
        
        if(IsMove(transform.position,hor,ver))
        {
            SetStateWalk();
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

    IEnumerator DebugModInputCo() 
    {

        while (Managers.Game.IsDebugMod && !PlayerIsDeathState()) 
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            pointerEventData.button = PointerEventData.InputButton.Left;
           
            var raycasts = Managers.Keyword.GetRaycastList(pointerEventData);
            float minKeywordDist = float.MaxValue;
            float minFrameDist = float.MaxValue;
           
            KeywordController nearKeyword = null;
            KeywordFrameBase nearFrame = null;
            
            for (int i = 0; i < raycasts.Count; ++i)
            {
                if (raycasts[i].gameObject.CompareTag(KEYWORD_TAG))
                {
                    var nowKeyword = raycasts[i].gameObject.GetComponent<KeywordController>();
                    if (nowKeyword.IsLock) 
                    {
                        continue;
                    }

                    var nowDist = (nowKeyword.RectTransform.position - Input.mousePosition).magnitude;
                    if (nowDist < minKeywordDist)
                    {
                        nearKeyword = nowKeyword;
                        minKeywordDist = nowDist;
                    }
                }
                else if (raycasts[i].gameObject.CompareTag(KEYWORD_FRAME_TAG))
                {
                    var nowFrame = raycasts[i].gameObject.GetComponent<KeywordFrameBase>();
                   
                    if (nowFrame.IsLock)
                    {
                        continue;
                    }
                    var nowDist = (nowFrame.RectTransform.position - Input.mousePosition).magnitude;

                    if (nowDist < minFrameDist)
                    {
                        nearFrame = nowFrame;
                        minFrameDist = nowDist;
                    }
                }
            }
           

            if (Managers.Keyword.CurDragKeyword == null) 
            {

                if (isFocusKeyword != nearKeyword)
                {
                    if (isFocusKeyword != null)
                    {
                        isFocusKeyword.OnPointerExit();
                        isFocusKeyword = null;
                    }
                    if (nearKeyword != null)
                    {
                        nearKeyword.OnPointerEnter();
                        isFocusKeyword = nearKeyword;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (isFocusKeyword != null)
                    {
                        isFocusKeyword.OnBeginDrag(pointerEventData);
                        //isFocusKeyword.OnPointerExit();
                        //isFocusKeyword = null;
                    }
                }

                if (isFocusFrame != null)
                {
                    isFocusFrame.OnPointerExit();
                    isFocusFrame = null;
                }

                yield return null;
                continue;
            }

            
            if (isFocusFrame != nearFrame)
            {
                if (isFocusFrame != null)
                {
                    isFocusFrame.OnPointerExit();
                    isFocusFrame = null;
                }
                if (nearFrame != null)
                {
                    nearFrame.OnPointerEnter();
                    isFocusFrame = nearFrame;
                }
            }


            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Managers.Keyword.CurDragKeyword.OnEndDrag(nearFrame);
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {

                Managers.Keyword.CurDragKeyword.OnDrag(pointerEventData);

            }

            yield return null;
        }

        debugModeInput = null;
    }
    public void EnterDebugMod()
    {
        Managers.Game.SetEnableDebugMod();

        if(debugModeInput != null) 
        {
            StopCoroutine(debugModeInput);
            
        }
        debugModeInput = StartCoroutine(DebugModInputCo());
        glitchEffectController.EnterDebugMod(() =>
        {
            animationController.OnEnterDebugMod();
        });
        interactionDetecter.SwitchDebugMod(true);
        

    }
    public void ExitDebugMod()
    {
        //SetstateStop();
        if (debugModeInput != null)
        {
            StopCoroutine(debugModeInput);
            debugModeInput = null;
        }
        
        glitchEffectController.ExitDebugMod(() => {
            
            interactionDetecter.SwitchDebugMod(false);
            animationController.OnExitDebugMod();
            isDebugButton();
        });
        if(isFocusKeyword != null) 
        {
            isFocusKeyword.OnPointerExit();
            isFocusKeyword = null;
        }
        if (isFocusFrame != null)
        {
            isFocusFrame.OnPointerExit();
            isFocusFrame = null;
        }

    }
    public void AnimIdleEnter()
    {
        animationController.SetIdleAnim(curDir);
    }
    public void AnimMoveEnter(Vector3 moveVec)
    {
        animationController.SetWalkAnim(curDir,moveVec);
    }
    public void AnimRunEnter(Vector3 moveVec)
    {
        animationController.SetRunAnim(curDir);
    }
    public void ClearMoveAnim()
    {
        AnimIdleEnter();
        rigid.velocity = Vector3.zero;
    }
    private bool PlayerIsStopState()
    {
        return playerStateController.CurState == PlayerStop.Instance;
    }
    private bool PlayerIsDeathState()
    {
        return playerStateController.CurState == PlayerDeath.Instance || playerStateController.CurState == PlayerBombDeath.Instance;
    }
    public bool IsOnSlope(Vector3 dir)
    {
        var dirPos = transform.position;
        Debug.DrawRay(dirPos, Vector3.down *(box.bounds.extents.y + stepHeight), Color.blue);
        if (Physics.Raycast(dirPos, Vector3.down, out slopeHit, (box.bounds.extents.y + stepHeight), slopeLayer))
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
        if (PlayerIsStopState()) 
        {
            return;
        }
        hp = hp - damage;
        playerUIController.SetHp(damage);
        if (hp <= 0)
        {
            SetstateDeath();
            return;
        }
        Managers.Sound.PlaySFX(Define.SOUND.HitPlayer);
        hitFeedback.PlayFeedbacks();
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
    public void SetStateWalk()
    {
        playerStateController.ChangeState(PlayerWalk.Instance);
    }
    public void SetStateRun()
    {
        playerStateController.ChangeState(PlayerRun.Instance);
    }
    public void SetstateStop()
    {
        playerStateController.ChangeState(PlayerStop.Instance);
    }
    public void SetstateBombDeath()
    {
        if (PlayerIsStopState())
        {
            return;
        }
        playerStateController.ChangeState(PlayerBombDeath.Instance);
    }
    public void SetstateDeath()
    {
        if (PlayerIsStopState())
        {
            return;
        }
        playerStateController.ChangeState(PlayerDeath.Instance);
    }
    #endregion
    #region WireEffect
    public void SetWireframeMaterial(List<Material> materials) 
    {
        wireframeMaskController.materials = materials;
    }
    public void AddWireframeMaterial(Material material)
    {
        wireframeMaskController.materials.Add(material);
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
        box.enabled = false;
        col.enabled = false;
        rigid.isKinematic = true;

        deathFeedback.transform.SetParent(null);
        deathFeedback.PlayFeedbacks();
        rigid.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
    public void PlayerDeathAnimPlay()
    {
        box.enabled = false;
        col.enabled = false;
        rigid.isKinematic = true;
        playerDeathTrigger.gameObject.SetActive(false);
        rigid.velocity = Vector3.zero;
        animationController.PlayerDeathAnimPlay();
    }
    public void PlayerDeathAnimEnd()
    {
        OpenDeathUI();
    }

    #endregion
    public void LoadData(GameData gamedata)
    {
        transform.position = gamedata.playerPos;
    }

    public void SaveData(GameData gameData)
    {
        gameData.playerPos = transform.position;
    }
}