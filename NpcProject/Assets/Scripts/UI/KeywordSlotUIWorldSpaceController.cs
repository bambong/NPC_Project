using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeywordSlotUIWorldSpaceController : UI_Base
{
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private TextMeshProUGUI keywordText;
    [SerializeField]
    private Image image;

    private readonly string DEFAULT_TEXT = "NULL";
    private readonly float Y_POS_REVISION_AMOUNT = 2f;
    public override void Init()
    {
        
    }
    public void SetSlotUI(Color color ,string text) 
    {
        this.keywordText.text = text;
        image.color = color;
    }
    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void Open(Transform parent)
    {
        transform.position = parent.position + Vector3.up * ((parent.GetComponent<Collider>().bounds.size.y / 2) + Y_POS_REVISION_AMOUNT);
        transform.rotation = Camera.main.transform.rotation;
 
        gameObject.SetActive(true);
    }
    public void Close() 
    {
        parent.gameObject.SetActive(false);
    }


    public void ResetSlotUI() 
    {
        image.color = Color.black;
        keywordText.text = DEFAULT_TEXT;
    }
}
