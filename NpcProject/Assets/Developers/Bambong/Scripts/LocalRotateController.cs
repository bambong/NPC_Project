using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalRotateController : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 100f;
    [SerializeField]
    private float distance = 0.4f;
    [SerializeField]
    private float floatDuration = 0.7f;
    private void Start()
    {
        transform.DOLocalMoveY(distance, floatDuration).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
    private void FixedUpdate()
    {
        var speed = rotateSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER);
        transform.Rotate(new Vector3(speed, speed, speed));
    }
}
