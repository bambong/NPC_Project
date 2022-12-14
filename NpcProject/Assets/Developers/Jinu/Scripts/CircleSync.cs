using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");

    [SerializeField]
    private Material WallMaterial;
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private LayerMask Mask;
    
    void Start() 
    {
        Camera = Camera.main;
    }

    void Update()
    {
        var dir = Camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);

        // Debug.DrawRay(transform.position, dir.normalized * 3000, Color.red);
        if(Physics.Raycast(ray, 3000, Mask))
        {
            WallMaterial.SetFloat(SizeID, 1);
        }
        else
        {
            WallMaterial.SetFloat(SizeID, 0);
        }

        var view = Camera.WorldToViewportPoint(transform.position);
        WallMaterial.SetVector(PosID, view + new Vector3(0, 0.1f));
    }
}
