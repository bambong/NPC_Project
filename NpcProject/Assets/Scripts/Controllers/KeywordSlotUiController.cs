using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordSlotUiController : UI_Base
{
    private readonly float Y_POS_REVISION_AMOUNT = 1;
    public override void Init()
    {
        
    }
    public void Open(Transform parent)
    {
        transform.position = parent.position + Vector3.up * ((parent.GetComponent<Collider>().bounds.size.y / 2) + Y_POS_REVISION_AMOUNT);
        transform.rotation = Camera.main.transform.rotation;

        gameObject.SetActive(true);
    }
    public void Close() 
    {
        gameObject.SetActive(false);
    }
}
