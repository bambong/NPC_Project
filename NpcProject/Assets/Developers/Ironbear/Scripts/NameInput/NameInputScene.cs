using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class NameInputScene : BaseScene
{
    [SerializeField]
    private Define.Scene targetScene;
    [SerializeField]
    private GameObject inputfieldPanel;

    public override void Clear()
    {

    }

    protected override void Init()
    {
        base.Init();
    }
}

