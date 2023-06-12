using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProgressUpdateController : MonoBehaviour
{

    [SerializeField]
    private float startDelay = 0.5f;
    [SerializeField]
    private UnityEvent onStart;

    private void Start()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(startDelay);
        seq.AppendCallback(()=>onStart?.Invoke());
        seq.Play();
    }

    public void OpenPurposePanel()
    {
        Managers.Game.Player.PurposePanel.Open();
    }
    public void ClosePurposePanel() 
    {
        Managers.Game.Player.PurposePanel.Close();
    }

    public void UpdateProgress(int progress) 
    {
        Managers.Data.UpdateProgress(progress);
    }

}
