using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public Material mat;
    public float thickness = 1.03f;
    public Color colorOutline;

    [ColorUsage(true, true)]

    private Renderer rend;

    void Start()
    {
        GameObject outlineObject = Instantiate(this.gameObject,transform.position,transform.rotation,transform);

        outlineObject.transform.localScale = new Vector3(1, 1, 1);
        Renderer rend = outlineObject.GetComponent<Renderer>();
        rend.material = mat;
        rend.material.SetFloat("_Thickness", thickness);
        rend.material.SetColor("_OutlineColor", colorOutline);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        rend.enabled = true;
        outlineObject.GetComponent<Collider>().enabled = false;
        outlineObject.GetComponent<OutlineEffect>().enabled = false;
        this.rend = rend;
    }
}
