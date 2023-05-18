using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public Material mat;
    public float thickness = 1.03f;
    
    [ColorUsage(true, true)]
    public Color colorOutline;

    [SerializeField]
    private GameObject prepabGo;
    
    private Renderer rend;
    private GameObject outLineGo;
    public GameObject OutLineGo { get => outLineGo; }
    void Start()
    {
        outLineGo = Instantiate(prepabGo, transform.position,transform.rotation,transform);
        outLineGo.SetActive(false);
        outLineGo.transform.localScale = new Vector3(1, 1, 1);
        Renderer rend = outLineGo.GetComponent<Renderer>();
        rend.material = mat;
        rend.material.SetFloat("_Thickness", thickness);
        rend.material.SetColor("_OutlineColor", colorOutline);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        rend.enabled = true;
        outLineGo.layer = 1;
        outLineGo.GetComponent<Collider>().enabled = false;
       // outLineGo.GetComponent<OutlineEffect>().enabled = false;
        this.rend = rend;
    }
}
