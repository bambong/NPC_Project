using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIcons : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private float floatingSpeed = 5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.Translate(Vector3.up * floatingSpeed * Time.deltaTime);
    }
}
