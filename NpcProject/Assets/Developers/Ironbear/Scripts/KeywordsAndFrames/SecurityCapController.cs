using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.Playables;

public class SecurityCapController : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject sphere;
    [SerializeField]
    private GameObject capPanel;
    [SerializeField]
    private GameObject locked;
    [SerializeField]
    private GameObject unlocked;
    [SerializeField]
    private GameObject card;

    private CanvasGroup lockedCanvas;
    private CanvasGroup unlockedCanvas;
    private CanvasGroup parentCanvas;
    private CanvasGroup cardCanvas;
    private SphereCollider sphereTrigger;

    private float fadeDuration = 0.5f;

    private void Start()
    {
        lockedCanvas = locked.GetComponent<CanvasGroup>();
        unlockedCanvas = unlocked.GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<CanvasGroup>();
        sphereTrigger = sphere.GetComponent<SphereCollider>();
        cardCanvas = card.GetComponent<CanvasGroup>();

        unlockedCanvas.alpha = 0f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();           
            draggedRect.DOLocalMove(new Vector3(300f, -200f, 0f), 0.1f).SetEase(Ease.OutQuad);

            cardCanvas.blocksRaycasts = false;
                       
            parentCanvas.interactable = false;
            parentCanvas.blocksRaycasts = false;
            Managers.Data.UpdateProgress(Managers.Data.Progress+1);
            CapUnlock();
        }
    }

    private void CapUnlock()
    {
        sphereTrigger.enabled = true;
        lockedCanvas.alpha = 0f;
        unlockedCanvas.alpha = 1f;

        DOVirtual.DelayedCall(1f, () =>
        {
            capPanel.GetComponent<CanvasGroup>().DOFade(0f, fadeDuration).OnComplete(() =>
            {
                capPanel.SetActive(false);
            });          
            Managers.Game.Player.SetStateIdle();
            var talk = new TalkEvent();
            talk = Managers.Talk.GetTalkEvent(10005);
            talk.Play();
        });


    }
}
