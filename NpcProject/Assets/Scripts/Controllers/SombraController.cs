using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void Init(Sprite sprite, Color color ,float time)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        spriteRenderer.DOFade(0, time).OnComplete(() => { Managers.Resource.Destroy(gameObject); });
    }

   

}
