using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DissolveDoorController : GuIdBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private UnityEvent onClear;

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
        onClear?.Invoke();
        animator.SetBool(OPEN_FLAG, true);
    }
}
