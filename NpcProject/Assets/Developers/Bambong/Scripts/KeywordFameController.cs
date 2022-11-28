using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordFameController : MonoBehaviour
{

    [SerializeField]
    private RectTransform rectTransform;

    private bool hasKeyword =false;

    public bool SetKeyWord(DragableController dragableController) 
    {
        if (hasKeyword) 
        {
            return false;
        }
        hasKeyword = true;  
        dragableController.SetPos(rectTransform.position);
        return true;
    }

}
