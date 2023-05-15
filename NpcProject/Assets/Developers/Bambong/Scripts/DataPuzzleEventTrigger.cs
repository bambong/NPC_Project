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
            return (!Managers.Data.isClearEvent(miniGameLevel.eventId) && Managers.Data.isAvaildProgress(miniGameLevel.progress));
        }
    }

    public void OnInteraction()
    {
        if (Managers.Data.isClearEvent(miniGameLevel.eventId)) // �̹� Ŭ���� �� �̺�Ʈ���� Ȯ��
        {
            return;
        }
        if (!Managers.Data.isAvaildProgress(miniGameLevel.progress)) // �̹� ���൵�� ��� ������
        {
            return;
        }
        Managers.Data.AddOnceEvent(miniGameLevel.eventId);
        Managers.Data.UpdateDataPuzzleLevel(miniGameLevel);
        Managers.Game.Player.SetstateStop();
        Managers.Scene.LoadScene(Define.Scene.DataPuzzle);
    }
}
