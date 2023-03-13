using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordWorldSlotLayoutController : UI_Base
{
    [SerializeField]
    private Transform panel;

    private readonly float Y_POS_REVISION_AMOUNT = 2f;
    public Transform Panel { get => panel; }
    private Collider entityColider;
    public override void Init()
    {

    }

    public void RegisterEntity(Transform entity) 
    {
        entityColider = entity.GetComponent<Collider>();
    }
    public void SortChild(float width) 
    {
        panel.rotation = Camera.main.transform.rotation;

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
       // panel.position = entityColider.transform.position + Vector3.up * ((entityColider.bounds.size.y / 2) + Y_POS_REVISION_AMOUNT);
      
        panel.position = entityColider.transform.position + Vector3.up * ((entityColider.bounds.size.y / 2) + Y_POS_REVISION_AMOUNT);
        panel.rotation = Camera.main.transform.rotation;
    }

}
