using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerKeywordLayoutTrigger : MonoBehaviour 
{
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private PlayerKeywordLayoutController controller;


    private void OnDisable()
    {
        controller.Close();
    }
    private void FixedUpdate()
    {
        // 마우스 위치를 가져옴
        Vector3 mousePos = Input.mousePosition;

        // 이미지 RectTransform을 가져옴
        RectTransform imageRect = rectTransform;

        // 이미지 내부에 있는지 확인
        if (RectTransformUtility.RectangleContainsScreenPoint(imageRect, mousePos))
        {
            Debug.Log("마우스가 이미지 내부에 있음");
            controller.Open();
    
        }
        else
        {
            controller.Close();
            Debug.Log("마우스가 이미지 외부에 있음");
        }
    }

}
