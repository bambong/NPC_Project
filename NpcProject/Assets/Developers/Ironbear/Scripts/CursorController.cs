using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursor;
    [SerializeField]
    private Texture2D cursorClicked;

 
    private void Awake()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;
    }


    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorClicked, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }
}
