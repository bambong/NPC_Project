using Spine.Unity;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public enum AnimDir 
    {
        Left,
        Right,
        Front,
        Back
    }
    
    [SerializeField]
    private SkeletonAnimation sideSpineAnim;
    [SerializeField]
    private SkeletonAnimation frontIdleSpineAnim;
    [SerializeField]
    private SkeletonAnimation backIdleSpineAnim;
    [SerializeField]
    private Animator frontBackMoveframeAnim;
    [Header("GhostEffect")]
    [SerializeField]
    private GhostEffectColorPickerController ghostColorPicker;
    [Header("FrameGhost")]
    [SerializeField]
    private List<GhostEffectController> ghostEffects;

    private List<GameObject> anims =new List<GameObject>();

    private AnimDir curDir = AnimDir.Front;
    private bool isMove = false;
    private bool isDebugMod = false;
    private void Awake()
    {
        frontBackMoveframeAnim.gameObject.GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        anims.Add(sideSpineAnim.gameObject);
        anims.Add(frontIdleSpineAnim.gameObject);
        anims.Add(backIdleSpineAnim.gameObject);
        anims.Add(frontBackMoveframeAnim.gameObject);
        
        
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

    private void EnableAnim(GameObject target)
    {
        for(int i =0; i<anims.Count; ++i) 
        {
            if(anims[i] != target) 
            {
                anims[i].SetActive(false);
            }
        }
        target.SetActive(true);
    }

    public void SetMoveAnim(AnimDir dir , Vector3 moveVec) 
    {
        if (isMove && curDir == dir)
        {
            return;
        }
        switch (dir)
        {
            case AnimDir.Left:
                EnableAnim(sideSpineAnim.gameObject);
                sideSpineAnim.AnimationState.SetAnimation(0, "animation", true);
                sideSpineAnim.skeleton.ScaleX = 1;
                break;
            case AnimDir.Right:
                EnableAnim(sideSpineAnim.gameObject);
                sideSpineAnim.AnimationState.SetAnimation(0, "animation", true);
                sideSpineAnim.skeleton.ScaleX = -1;
                break;
            case AnimDir.Front:
                //if (isDebugMod) 
                //{
                //    ghostEffects[0].Open();
                //}
                EnableAnim(frontBackMoveframeAnim.gameObject);
                frontBackMoveframeAnim.SetBool("IsFront", true);
                break;
            case AnimDir.Back:
                //if (isDebugMod)
                //{
                //    ghostEffects[0].Close();
                //}
               
                EnableAnim(frontBackMoveframeAnim.gameObject);
                frontBackMoveframeAnim.SetBool("IsFront", false);
                break;
        }
        curDir = dir;
        isMove = true;
    }
    public void SetIdleAnim(AnimDir dir) 
    {
        if (!isMove && curDir == dir)
        {
            return;
        }
        switch (dir)
        {
            case AnimDir.Left:
                EnableAnim(sideSpineAnim.gameObject);
                sideSpineAnim.AnimationState.SetAnimation(0, "idel", true);
                sideSpineAnim.skeleton.ScaleX = 1;
                break;
            case AnimDir.Right:
                EnableAnim(sideSpineAnim.gameObject);
                sideSpineAnim.AnimationState.SetAnimation(0, "idel", true);
                sideSpineAnim.skeleton.ScaleX = -1;
                break;
            case AnimDir.Front:
                EnableAnim(frontIdleSpineAnim.gameObject);
                break;
            case AnimDir.Back:
                EnableAnim(backIdleSpineAnim.gameObject);
                break;
        }
        curDir = dir;
        isMove = false;
    }
    public void PlayerHitEffectPlay() 
    {
    
    }
}
