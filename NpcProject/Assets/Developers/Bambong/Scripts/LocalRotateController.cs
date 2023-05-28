using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalRotateController : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 100f;
    private void FixedUpdate()
    {
        var speed = rotateSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER);
        transform.Rotate(new Vector3(speed, speed, speed));
    }
}
