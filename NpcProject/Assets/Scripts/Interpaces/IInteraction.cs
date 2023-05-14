using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteraction
{
    public GameObject Go { get; }
    public void OnInteraction();
}
