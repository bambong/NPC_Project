using UnityEngine;
using DG.Tweening;

public class SettingsPanelController : UI_Base
{
    [SerializeField]
    private GameObject settingPanel;

    private bool isOpen = false;
    private float animDuration = 0.5f;

    private Sequence sq;

    public override void Init()
    {
    }

    private void Start()
    {
        sq = DOTween.Sequence()
            .SetAutoKill(false)
            .Pause()
            .Append(settingPanel.transform.DOScale(Vector3.zero, 0f));

        settingPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isOpen)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
            }
        }
    }

    private void OpenPanel()
    {
        isOpen = true;
        settingPanel.SetActive(true);

        sq.Restart();
        sq.Append(settingPanel.transform.DOScale(Vector3.one, animDuration));
    }

    private void ClosePanel()
    {
        isOpen = false;
        sq.Append(settingPanel.transform.DOScale(Vector3.zero, animDuration));
    }
}
