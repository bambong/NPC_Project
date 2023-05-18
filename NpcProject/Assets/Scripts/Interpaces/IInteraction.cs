using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteraction
{
    public GameObject Go { get; }
    public bool IsInteractAble { get; }
    public void OnInteraction();
}
