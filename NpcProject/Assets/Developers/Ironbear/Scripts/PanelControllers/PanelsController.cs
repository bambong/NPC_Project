using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelsController : UI_Base
{
    [SerializeField]
    private CanvasGroup canvas;
    [SerializeField]
    private GameObject logoPanel;
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject puzzlePanel;
    [SerializeField]
    private GameObject textPanel;

    [SerializeField]
    private Texture2D noramlCursor;

    private float fadeDuration = 1f;

    private bool isLogo = false;
    private bool isStart = false;
    private bool isPuzzle = false;
    private bool isText = false;


    public override void Init()
    {
    }

    private void Start()
    {
        Cursor.SetCursor(noramlCursor, Vector2.zero, CursorMode.ForceSoftware);
        Cursor.lockState = CursorLockMode.Confined;
        Camera.main.ScreenPointToRay(Input.mousePosition);

        logoPanel.SetActive(true);
        startPanel.SetActive(false);
        puzzlePanel.SetActive(false);
        textPanel.SetActive(false);
        isLogo = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CanvasFadeOut()
    {
        canvas.alpha = 1f;
        canvas.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad).OnComplete(() => {
            if(isLogo)
            {
                logoPanel.SetActive(false);
                startPanel.SetActive(true);
                isLogo = false;
                isStart = true;
            }
            else if(isStart)
            {
                startPanel.SetActive(false);
                puzzlePanel.SetActive(true);
                isStart = false;
                isPuzzle = true;
            }
            else if(isPuzzle)
            {
                puzzlePanel.SetActive(false);
                textPanel.SetActive(true);
                isPuzzle = false;
                isText = true;
            }
            
            CanvasFadeIn(); 
        });
    }

    public void Noninteractive()
    {
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        Image clickedImage = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        CanvasGroup clickedCanvasGroup = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<CanvasGroup>();

        clickedCanvasGroup.interactable = false;
        clickedCanvasGroup.blocksRaycasts = false;
        clickedImage.raycastTarget = false;
        clickedButton.interactable = false;
    }

    private void CanvasFadeIn()
    {
        canvas.alpha = 0f;
        canvas.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);
    }
}
