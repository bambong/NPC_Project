using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : MonoBehaviour
{
    [SerializeField]
    private Animator golemAnim;

    public void Idle()
    {
        golemAnim.SetBool("Motion", false);
    }

    public void Motion()
    {
        golemAnim.SetBool("Motion", true);
    }      
}
