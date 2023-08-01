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
        //controller.Close();
    }


    private void Update()
    {
        //// 마우스 위치를 가져옴
        //Vector3 mousePos = Input.mousePosition;

        //// 이미지 RectTransform을 가져옴
        //RectTransform imageRect = rectTransform;



        //// 이미지 내부에 있는지 확인
        //if (RectTransformUtility.RectangleContainsScreenPoint(imageRect, mousePos))
        //{
        //    controller.Open();

        //}
        //else
        //{
        //    controller.Close();
        //}

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (controller.IsOpen) 
            {
                controller.Close();
            }
            else
            {
                controller.Open();
            }
        }
    }

 }


