using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineGhostColorController : GhostEffectController
{
    [SerializeField]
    private SkeletonGhost ghost;

    public override void Close()
    {
        ghost.enabled = false;
        enabled = false;
    }

    public override void Open()
    {
        ghost.enabled = true;
        enabled = true;
    }

    void FixedUpdate()
    {
        ghost.color = colorPicker.CurrentColor;
    }
}
