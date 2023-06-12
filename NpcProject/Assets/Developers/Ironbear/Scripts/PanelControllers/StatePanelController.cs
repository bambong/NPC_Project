using UnityEngine;
using TMPro;
using DG.Tweening;

public class StatePanelController : MonoBehaviour
{
    public Speaker player;

    [SerializeField]
    private TMP_Text text1;
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject arrow;

    void Start()
    {
        door.transform.localPosition = new Vector3(1f, 0f, -68.5f);
        door.transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        UpdateName();
    }

    private void UpdateName()
    {
        string dialogue = "현재 <Player>의 근무 상태를 변경합니다.";
        dialogue = dialogue.Replace("<Player>", player.charName);
        text1.text = dialogue;
    }

    public void DoorOpen()
    {
        door.transform.localPosition = Vector3.zero;
        door.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void FloorArrowEffect()
    {
        arrow.SetActive(true);
        Sequence arrowSeq = DOTween.Sequence();

        arrowSeq.Append(arrow.transform.DOLocalMoveX(arrow.transform.localPosition.x + 1f, 1f).SetEase(Ease.InOutQuad));
        arrowSeq.Append(arrow.transform.DOLocalMoveX(arrow.transform.localPosition.x, 1f).SetEase(Ease.InOutQuad));
        arrowSeq.SetLoops(-1);
        arrowSeq.Play();
    }
}
