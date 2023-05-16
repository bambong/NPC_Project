using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[Serializable]
public class TMPTextSwitcherBehaviour : PlayableBehaviour
{
    public Color color = Color.white;
    public float fontSize = 24;
    [TextArea]
    public string text;
}
