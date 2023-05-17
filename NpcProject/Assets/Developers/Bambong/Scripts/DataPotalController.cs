using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPotalController : PotalScript
{
    [SerializeField]
    private int needCount =2;
    [SerializeField]
    private Vector3 desireScale;

    private int curCount = 0;
    private bool isOpen = false;
    public override bool IsInteractAble { get => isOpen; }

    public void Open() 
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(desireScale, 1f));
        seq.OnComplete(() => { isOpen = true; });
        seq.Play();
    }
    public void AddCount() 
    {
        curCount++;
        if(curCount >= needCount) 
        {
            Open();
        }
    }
}
