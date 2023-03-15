using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LookAtCameraController : UI_Base
{
    private BoxCollider entityColider;

    public override void Init()
    {
    }

    public void RegisterEntity(Transform entity)
    {
        entityColider = entity.GetComponent<BoxCollider>();
    }
    private void Update()
    {
        var camPos = Camera.main.transform.position;
        var camdir = camPos - entityColider.transform.position;
        transform.position = entityColider.bounds.center + camdir.normalized * entityColider.bounds.extents.magnitude;
        transform.rotation = Camera.main.transform.rotation;
    }
}
