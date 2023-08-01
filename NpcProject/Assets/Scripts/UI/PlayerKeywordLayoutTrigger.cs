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
        //// ���콺 ��ġ�� ������
        //Vector3 mousePos = Input.mousePosition;

        //// �̹��� RectTransform�� ������
        //RectTransform imageRect = rectTransform;



        //// �̹��� ���ο� �ִ��� Ȯ��
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


