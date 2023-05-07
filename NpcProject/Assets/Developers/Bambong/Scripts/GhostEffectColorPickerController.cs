using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Platform;
using UnityEngine;

public class GhostEffectColorPickerController : MonoBehaviour
{
    [SerializeField]
    private List<Color> colors;
    
    [SerializeField]
    private float colorTime = 0.2f;

    private float curTime = 0;
    private int colorIndex = 0;

    private Color culColor = Color.white;
    public Color CurrentColor { get => culColor; }

    public void OnEnterDebugMod() => Managers.Scene.CurrentScene.StartCoroutine(ColorUpdate());
    IEnumerator ColorUpdate()
    {
        while (Managers.Game.IsDebugMod)
        {
            curTime += Time.deltaTime;
            if (curTime >= colorTime)
            {
                curTime = 0;
                colorIndex = (colorIndex + 1) % colors.Count;
            }
            int nextIndex = (colorIndex + 1) % colors.Count;
            culColor = Color.Lerp(colors[colorIndex], colors[nextIndex], curTime * (1 / colorTime));
            yield return null;

        }
    }

}
