using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraActiveController : MonoBehaviour
{
    [SerializeField]
    private int progress;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Define.Scene transitionScene;

    public void Start()
    {
        if(Managers.Data.Progress == progress)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SideIdle()
    {
        anim.Play("Meria_Idle_Side");
    }    

    public void LoadScene()
    {
        Managers.Game.Player.SetstateStop();
        Managers.Scene.LoadScene(transitionScene);
    }
}
