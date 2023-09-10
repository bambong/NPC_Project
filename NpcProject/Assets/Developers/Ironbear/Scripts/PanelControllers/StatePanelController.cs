using UnityEngine;
using DG.Tweening;
using TMPro;

public class StatePanelController : MonoBehaviour
{
    public Speaker player;

    [SerializeField]
    private TMP_Text text1;
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject mouse;

    private bool isAnim = false;
    private float effectDuration = 2f;


    void Start()
    {
        door.transform.localPosition = new Vector3(1f, 0f, -68.5f);
        door.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        mouse.GetComponent<CanvasGroup>().alpha = 0f;

        UpdateName();
        Anim();
    }

    private void Awake()
    {
        
    }

    private void Anim()
    {
        if (isAnim)
        {
            return;
        }

        isAnim = true;

        Sequence seq = DOTween.Sequence();

        seq.Prepend(mouse.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetEase(Ease.OutQuad));
        seq.Append(mouse.transform.DOLocalMoveX(mouse.transform.localPosition.x + 270f, effectDuration).SetEase(Ease.OutQuad));
        seq.OnComplete(() =>
        {
            mouse.GetComponent<CanvasGroup>().DOFade(0f, 1f).SetEase(Ease.OutQuad);
            isAnim = false;
        });

        seq.Play();
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
        Managers.Data.UpdateProgress(1);
    }
}
