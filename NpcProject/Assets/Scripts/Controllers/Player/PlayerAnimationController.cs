using Spine.Unity;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public enum MoveDir 
    {
        Left,
        Right,
        Front,
        Back
    }
    public enum PlayerDir
    {
        PlayerSide,
        PlayerFront,
        PlayerBack
    }
    public enum AnimTag
    {
        _Idle,
        _Walk,
        _Run
    }
    [SerializeField]
    private Animator animatorController;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [Header("GhostEffect")]
    [SerializeField]
    private GhostEffectColorPickerController ghostColorPicker;
    [Header("FrameGhost")]
    [SerializeField]
    private List<GhostEffectController> ghostEffects;

    private MoveDir curDir = MoveDir.Front;
    private bool isMove = false;
    private bool isDebugMod = false;

    private void Awake()
    {
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        foreach (var item in ghostEffects)
        {
            item.Init(ghostColorPicker);
        }
    }
    public void OnEnterDebugMod() 
    {
        isDebugMod = true;
        ghostColorPicker.OnEnterDebugMod();
        foreach(var item in ghostEffects) 
        {
            item.Open();
        }
    }
    public void OnExitDebugMod() 
    {
        isDebugMod = false;
        foreach (var item in ghostEffects)
        {
            item.Close();
        }
    }
    private void AnimationOrderDirCheck(StringBuilder str , MoveDir dir) 
    {
        switch (dir)
        {
            case MoveDir.Left:
                str.Append(PlayerDir.PlayerSide.ToString());
                break;
            case MoveDir.Right:
                str.Append(PlayerDir.PlayerSide.ToString());
                break;
            case MoveDir.Front:
                str.Append(PlayerDir.PlayerFront.ToString());
                break;
            case MoveDir.Back:
                str.Append(PlayerDir.PlayerBack.ToString());
                break;
        }
    }
    private void FlipUpdate(MoveDir dir) 
    {
        var scale = spriteRenderer.transform.localScale;
        if (dir == MoveDir.Right)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }
        spriteRenderer.transform.localScale = scale;
    }
   
    public void SetIdleAnim(MoveDir dir) 
    {
        if (!isMove && curDir == dir)
        {
            return;
        }

        StringBuilder animOrder = new StringBuilder();

        FlipUpdate(dir);
        AnimationOrderDirCheck(animOrder, dir);
        animOrder.Append(AnimTag._Idle.ToString());
        animatorController.Play(animOrder.ToString());

        curDir = dir;
        isMove = false;
    
    }
    public void SetWalkAnim(MoveDir dir, Vector3 moveVec)
    {
        if (isMove && curDir == dir)
        {
            return;
        }

        StringBuilder animOrder = new StringBuilder();

        FlipUpdate(dir);
        AnimationOrderDirCheck(animOrder, dir);
        animOrder.Append(AnimTag._Walk.ToString());
        animatorController.Play(animOrder.ToString());

        curDir = dir;
        isMove = true;
    }

    public void SetRunAnim(MoveDir dir)
    {
        if (isMove && curDir == dir)
        {
            return;
        }

        StringBuilder animOrder = new StringBuilder();

        FlipUpdate(dir);
        AnimationOrderDirCheck(animOrder, dir);
        animOrder.Append(AnimTag._Run.ToString());
        animatorController.Play(animOrder.ToString());

        curDir = dir;
        isMove = true;
    }
    public void PlayerDeathAnimPlay() 
    {
        animatorController.SetBool("IsDead", true);
    }
}
