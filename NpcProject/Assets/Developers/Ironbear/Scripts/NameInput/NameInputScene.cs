using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameInputScene : BaseScene
{
    [SerializeField]
    private Define.Scene targetScene;

    public override void Clear()
    {
      
    }

    protected override void Init()
    {
        base.Init();

        //Managers.Scene.LoadScene(targetScene);
    }
}

