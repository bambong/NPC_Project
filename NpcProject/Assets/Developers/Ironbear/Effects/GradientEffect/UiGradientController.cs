using UnityEngine;

public class UiGradientController : MonoBehaviour
{
    public Material gradientMat;
    public Color leftColor;
    public Color rightColor;

    private void Start()
    {
        gradientMat.SetColor("_Color", leftColor);
        gradientMat.SetColor("_Color", rightColor);
    }
}
