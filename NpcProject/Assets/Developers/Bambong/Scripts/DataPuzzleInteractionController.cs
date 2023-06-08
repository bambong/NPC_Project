using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DataPuzzleInteractionController : MonoBehaviour, IInteraction
{
    [SerializeField]
    private MiniGameLevelData miniGameLevel;

    [SerializeField]
    private List<CoreCubeController> cubeControllers = new List<CoreCubeController>();
    [SerializeField]
    private GuIdBehaviour successEventGuid;

    [SerializeField]
    private UnityEvent onSuccess;

    public GameObject Go => gameObject;
    public bool IsInteractAble
    {
        get
        {
            return (!Managers.Data.IsClearEvent(miniGameLevel.guId));
        }
    }

    private void Start()
    {
       // SuccessDataCube();
        Managers.Keyword.OnEnterDebugModEvent += OpenDataCube;
        Managers.Keyword.OnExitDebugModEvent += CloseDataCube;
        if (Managers.Data.IsClearEvent(miniGameLevel.guId)) 
        {

            if (!Managers.Data.IsClearEvent(successEventGuid.GuId))
            {
                Managers.Data.ClearEvent(successEventGuid.GuId);
                SuccessDataCube();
                var seq = DOTween.Sequence();
                seq.AppendInterval(5f);
                seq.AppendCallback(() => { onSuccess?.Invoke(); });
            }
            else 
            {
                for (int i = 0; i < cubeControllers.Count; ++i)
                {
                    cubeControllers[i].gameObject.SetActive(false);
                }
                onSuccess?.Invoke();
                Managers.Keyword.OnEnterDebugModEvent -= OpenDataCube;
                Managers.Keyword.OnExitDebugModEvent -= CloseDataCube;
            }
        }
    }

    public void OpenDataCube() 
    {
        for (int i = 0; i < cubeControllers.Count; ++i)
        {
            cubeControllers[i].OpenAll();
        }
    }
    public void CloseDataCube() 
    {
        for(int i =0; i < cubeControllers.Count; ++i)
        {
            cubeControllers[i].CloseAll();
        }
    }
    public void SuccessDataCube()
    {
        for (int i = 0; i < cubeControllers.Count; ++i)
        {
            cubeControllers[i].SuccessAll();
        }
    }
    public void OnInteraction()
    {
        if (Managers.Data.IsClearEvent(miniGameLevel.guId)) // 이미 클리어 된 이벤트인지 확인
        {
            return;
        }
        Managers.Data.UpdateDataPuzzleLevel(miniGameLevel);
        Managers.Game.Player.SetstateStop();
        Managers.Scene.LoadScene(Define.Scene.DataPuzzle);
    }
}
