using UnityEngine;
using UnityEngine.UI;

public class BasePanelController : UI_Base
{
    [SerializeField]
    protected RectTransform panel;

    protected bool isOpen;

    public bool IsOpen { get => isOpen;  }

    public override void Init()
    {
        panel.gameObject.SetActive(false);
    }
    public void Open()
    {
        if (isOpen)
        {
            return;
        }
        isOpen = true;
        OnOpen();
    }
    public void Close()
    {
        if (!isOpen)
        {
            return;
        }
        isOpen = false;
        OnClose();
    }

    protected virtual void OnOpen()
    {
        panel.gameObject.SetActive(true);
    }
    protected virtual void OnClose()
    {
        panel.gameObject.SetActive(false);
    }
}
