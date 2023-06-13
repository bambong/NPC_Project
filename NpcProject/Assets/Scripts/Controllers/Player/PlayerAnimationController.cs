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

    private PlayerDir curPlayerDir;
    private AnimTag curAnimTag;
    private bool isMove = false;
    private bool isDebugMod = false;

    private void Awake()
    {
       // spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

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
    private PlayerDir GetPlayerDir(MoveDir dir) 
    {
        switch (dir)
        {
            case MoveDir.Left:
                return PlayerDir.PlayerSide;
            case MoveDir.Right:
                return PlayerDir.PlayerSide;
            case MoveDir.Front:
                return PlayerDir.PlayerFront;
            case MoveDir.Back:
                return PlayerDir.PlayerBack;
        }
        return PlayerDir.PlayerSide;
    }

    private void AnimationOrderDirCheck(StringBuilder str , MoveDir dir) 
    {
        curPlayerDir = GetPlayerDir(dir);
        str.Append(curPlayerDir.ToString());
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
        FlipUpdate(dir);
        if (curAnimTag == AnimTag._Idle && curPlayerDir == GetPlayerDir(dir))
        {
            return;
        }

        StringBuilder animOrder = new StringBuilder();

        AnimationOrderDirCheck(animOrder, dir);
        animOrder.Append(AnimTag._Idle.ToString());
        animatorController.Play(animOrder.ToString());
        curAnimTag = AnimTag._Idle;

        isMove = false;
    
    }
    public void SetWalkAnim(MoveDir dir, Vector3 moveVec)
    {
        FlipUpdate(dir);
        if (curAnimTag == AnimTag._Walk && curPlayerDir == GetPlayerDir(dir))
        {
            return;
        }
        StringBuilder animOrder = new StringBuilder();

        AnimationOrderDirCheck(animOrder, dir);
        animOrder.Append(AnimTag._Walk.ToString());
        animatorController.Play(animOrder.ToString());
        curAnimTag = AnimTag._Walk;
        isMove = true;
    }

    public void SetRunAnim(MoveDir dir)
    {
        FlipUpdate(dir);
        if (curAnimTag == AnimTag._Run && curPlayerDir == GetPlayerDir(dir))
        {
            return;
        }

        StringBuilder animOrder = new StringBuilder();

        AnimationOrderDirCheck(animOrder, dir);
        animOrder.Append(AnimTag._Run.ToString());
        animatorController.Play(animOrder.ToString());
        curAnimTag = AnimTag._Run;
 
        isMove = true;
    }
    public void PlayerDeathAnimPlay() 
    {
        animatorController.SetTrigger("IsDead");
    }

    public void PlayerDeathAnimEnd() 
    {
        Managers.Game.Player.PlayerDeathAnimEnd();
    }
}
