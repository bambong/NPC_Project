using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ColorState
{
    public E_WIRE_COLOR_MODE mode;
    [ColorUsage(true,true)]
    public Color color;
}
