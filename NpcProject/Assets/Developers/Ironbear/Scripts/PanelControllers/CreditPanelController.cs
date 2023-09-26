using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;



public class CreditPanelController : MonoBehaviour
{
    [System.Serializable]
    public class CreditItem
    {
        public string title;
        public string name;
        public Sprite sprite;
    }

    [SerializeField]
    private List<CreditItem> creditItems = new List<CreditItem>();
    [SerializeField]
    private CanvasGroup txtCanvas;

    private void Start()
    {
        txtCanvas = txtCanvas.GetComponent<CanvasGroup>();

        txtCanvas.alpha = 0f;
    }
}
