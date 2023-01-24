using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordWorldSlotLayoutController : UI_Base
{
    [SerializeField]
    private Transform panel;

    private readonly float Y_POS_REVISION_AMOUNT = 2f;
    public Transform Panel { get => panel; }
    public override void Init()
    {

    }


    public void SortChild(float width) 
    {
        var parent = transform.parent;
        panel.position = parent.position + Vector3.up * ((parent.GetComponent<Collider>().bounds.size.y / 2) + Y_POS_REVISION_AMOUNT);
        panel.rotation = Camera.main.transform.rotation;

        float startPos = ((panel.childCount/2)-0.5f) * width * -1;

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
        panel.rotation = Camera.main.transform.rotation;
    }

}
