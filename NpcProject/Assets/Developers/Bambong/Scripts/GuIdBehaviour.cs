using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuIdBehaviour : MonoBehaviour
{
    [Header("GUID")]
    [SerializeField]
    protected string guId;

    public string GuId { get => guId; }

    [ContextMenu("Generate GUID")]
    private void GenerateGuid()
    {
        guId = System.Guid.NewGuid().ToString();
    }
}
