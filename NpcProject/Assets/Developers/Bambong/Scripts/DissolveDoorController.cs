using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDoorController : GuIdBehaviour
{
    [SerializeField]
    private Animator animator;
    private readonly string OPEN_FLAG = "IsOpen";
    protected override void Start()
    {
        base.Start();
        if (Managers.Data.IsClearEvent(guId)) 
        {
            Open();
        }
    }
    [ContextMenu("Open")]
    public void Open() 
    {
        if (!Managers.Data.IsClearEvent(guId))
        {
            Managers.Data.ClearEvent(guId);
        }
        animator.SetBool(OPEN_FLAG, true);
    }
}
