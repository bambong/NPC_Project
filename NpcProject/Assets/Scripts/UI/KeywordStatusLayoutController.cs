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
    protected Transform debugIcon;
    public Transform Panel { get => panel; }

    private BoxCollider entityColider;
    private List<KeywordStatusUiController> keywordStatusUis = new List<KeywordStatusUiController>();
    public List<KeywordStatusUiController> KeywordStatusUis { get => keywordStatusUis;}
    private readonly float ELEMENT_WIDTH = 0.6f;
    public override void Init()
    {

    }
    public virtual void Opne() 
    {
        panel.gameObject.SetActive(true);
        debugIcon.gameObject.SetActive(true);
    }
    public virtual void Close() 
    {
        panel.gameObject.SetActive(false);
        debugIcon.gameObject.SetActive(false);
    }

    public void AnimClose(float time) 
    {
        panel.DOKill();
        debugIcon.DOKill();
        panel.DOScale(Vector3.zero, time);
        debugIcon.DOScale(Vector3.zero, time);

    }
    public void AnimOpne(float time)
    {
        panel.DOKill();
        debugIcon.DOKill();
        panel.DOScale(Vector3.one, time);
        debugIcon.DOScale(Vector3.one, time);

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
        panel.rotation = Quaternion.Euler(new Vector3(0,180,0)) * Camera.main.transform.rotation ;

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
