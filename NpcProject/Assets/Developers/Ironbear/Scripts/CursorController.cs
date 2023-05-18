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
        Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private void Update()
    {
        Ray();
    }

    private void Ray()
    {
        Ray currentRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(currentRay, out hitInfo, Mathf.Infinity, 1 << 8))
        {
            OnMouseEnter();
        }
        else
        {
            OnMouseExit();
        }
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
