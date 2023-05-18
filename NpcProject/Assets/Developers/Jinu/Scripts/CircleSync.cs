using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");

    [SerializeField]
    private Material[] wallMaterial;
    [SerializeField]
    private Camera matCamera;
    [SerializeField]
    private LayerMask mask;

    private float matLength;
    
    void Start() 
    {
        matCamera = Camera.main;
        matLength = wallMaterial.Length;        
    }

    void Update()
    {
        var dir = matCamera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);

        // Debug.DrawRay(transform.position, dir.normalized * 3000, Color.red);
        if(Physics.Raycast(ray, 3000, mask))
        {
            for(int i = 0; i < matLength; i++)
            {
                wallMaterial[i].SetFloat(SizeID, 1);
            }            
        }
        else
        {
            for (int i = 0; i < matLength; i++)
            {
                wallMaterial[i].SetFloat(SizeID, 0);
            }
        }

        var view = matCamera.WorldToViewportPoint(transform.position);
        for (int i = 0; i < matLength; i++)
        {
            wallMaterial[i].SetVector(PosID, view + new Vector3(0, 0.1f));
        }
        // WallMaterial[0].SetVector(PosID, view + new Vector3(0, 0.1f));
    }
}
