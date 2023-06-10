using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordStatusLayoutController : UI_Base
{
    [SerializeField]
    protected Transform panel;
    [SerializeField]
    private CanvasGroup canvasGroup;
    public Transform Panel { get => panel; }

    private bool isOpen;
    private BoxCollider entityColider;
    private List<KeywordStatusUiController> keywordStatusUis = new List<KeywordStatusUiController>();
    public List<KeywordStatusUiController> KeywordStatusUis { get => keywordStatusUis;}
    
    private readonly float ELEMENT_WIDTH = 1.2f;
    private const float OPEN_ANIM_TIME = 0.5f;
    private const float CLOSE_ANIM_TIME = 0.5f;
    public override void Init()
    {

    }
    public void Open(float time = OPEN_ANIM_TIME)
    {
        if (isOpen)
        {
            return;
        }
        isOpen = true;
        panel.gameObject.SetActive(true);
        canvasGroup.DOKill();
        var animTime = time * (1 - canvasGroup.alpha);
        canvasGroup.DOFade(1, animTime).SetUpdate(true);
    }
    public void Close(float time = CLOSE_ANIM_TIME)
    {

        if (!isOpen)
        {
            return;
        }
        isOpen = false;
        canvasGroup.DOKill();
        var animTime = time * canvasGroup.alpha;
        canvasGroup.DOFade(0, animTime).SetUpdate(true).OnComplete(() => { panel.gameObject.SetActive(false); });
    }
 
    public void RegisterEntity(Transform entity ,int count) 
    {
        entityColider = entity.GetComponent<BoxCollider>();
        for(int i = 0; i < count; ++i) 
        {
            MakeWorldSlot();
        }
        SortChild(ELEMENT_WIDTH);
    }
    private KeywordStatusUiController MakeWorldSlot() 
    {
        var worldSlot = Managers.UI.MakeWorldSpaceUI<KeywordStatusUiController>(panel, "KeywordStatusUi");
        keywordStatusUis.Add(worldSlot);
        return worldSlot;
    }
    public void SortChild(float width) 
    {
       // panel.rotation = Quaternion.Euler(new Vector3(0,180,0)) * Camera.main.transform.rotation ;

        float startPos = ((panel.childCount/2f)-0.5f) * width * -1;

        for(int i = 0; i < panel.childCount; ++i)
        {
            var child = panel.GetChild(i);
            var pos = Vector3.zero;
            pos.x = startPos;
            child.localPosition = pos;
            child.localRotation = Quaternion.identity;
            startPos += width;
        }
    }

    private void Update()
    {
        var camPos = Camera.main.transform.position;
        var camdir = camPos - entityColider.transform.position;
        transform.position = entityColider.bounds.center + camdir.normalized * entityColider.bounds.extents.magnitude;
        transform.rotation = Camera.main.transform.rotation;
    }

}
