using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPuzzleEventTrigger : MonoBehaviour, IInteraction
{
    [SerializeField]
    private MiniGameLevelData miniGameLevel;

    public GameObject Go => gameObject;
    public bool IsInteractAble
    {
        get
        {
            return (!Managers.Data.IsClearEvent(miniGameLevel.guId) && Managers.Data.isAvaildProgress(miniGameLevel.progress));
        }
    }

    public void OnInteraction()
    {
        if (Managers.Data.IsClearEvent(miniGameLevel.guId)) // 이미 클리어 된 이벤트인지 확인
        {
            return;
        }
        if (!Managers.Data.isAvaildProgress(miniGameLevel.progress)) // 이미 진행도를 벗어난 것인지
        {
            return;
        }
        Managers.Data.UpdateDataPuzzleLevel(miniGameLevel);
        Managers.Game.Player.SetstateStop();
        Managers.Scene.LoadScene(Define.Scene.DataPuzzle);
    }
}
