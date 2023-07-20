using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ILaserAction
{
    void OnLaserHit();
    void OffLaserHit();
}
