using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLaser : MonoBehaviour
{    
    [SerializeField]
    private GameObject correctObject;
    [Header("LaserProperty")]
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    public float maxLength = 300f; //laser max length
    [SerializeField]
    public float laserWidth = 0.1f; //laser width    
    [Header("Laser")]
    [SerializeField]
    private GameObject baseLaserStart;
    [SerializeField]
    private GameObject baseLaserEnd;
    [SerializeField]
    private GameObject hitLaserStart;
    [SerializeField]
    private GameObject hitLaserEnd;
    [SerializeField]
    private Material hitMat;

    private GameObject hitEffect;
    private Renderer curMat;
    private Material originMat;
    

    private bool enter = false;

    private void Start()
    {
        hitEffect = baseLaserEnd;

        curMat = GetComponent<Renderer>();
        originMat = curMat.material;

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
    }

    private void Update()
    {
         ShootLaser();
    }

    private void ShootLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxLength))
        {
            if(hit.collider.CompareTag("Laser"))
            {
                if(hit.collider.gameObject.name == correctObject.name)
                {
                    if(!enter)
                    {
                        LaserEnter();
                        enter = true;
                    }
                }
            }
            else
            {
                LaserExit();
                enter = false;
            }            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
            hitEffect.gameObject.transform.position = hit.point;
            
        }
        else
        {
            if(enter)
            {
                LaserExit();
                enter = false;
            }            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxLength);
            hitEffect.gameObject.transform.localPosition = transform.forward * maxLength;
        }
    }

    public void LaserEnter()
    {

        curMat.material = hitMat;
        hitEffect = hitLaserEnd;
        baseLaserStart.SetActive(false);
        baseLaserEnd.SetActive(false);
        hitLaserStart.SetActive(true);
        hitLaserEnd.SetActive(true);
    }
    
    public void LaserExit()
    {
        curMat.material = originMat;
        hitEffect = baseLaserEnd;
        baseLaserStart.SetActive(true);
        baseLaserEnd.SetActive(true);
        hitLaserStart.SetActive(false);
        hitLaserEnd.SetActive(false);
    }

    //public void SetLaserColor(ParticleSystem particle)
    //{
    //    ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particle.colorOverLifetime;
    //    Gradient gradient = colorOverLifetime.color.gradient;

    //    GradientColorKey[] colorKeys = gradient.colorKeys;
    //    int lastIndex = colorKeys.Length - 1;
    //    colorKeys[0].color = Color.white;
    //    colorKeys[0].time = 0f;

    //    colorKeys[1].color = Color.white;
    //    colorKeys[1].time = 0.492f;

    //    colorKeys[2].color = new Vector4(255, 0, 255);
    //    colorKeys[2].time = 0.761f;

    //    colorKeys[3].color = new Vector4(95, 25, 255);
    //    colorKeys[3].time = 0.998f;

    //    gradient.colorKeys = colorKeys;

    //    colorOverLifetime.color = gradient;
    //}
}
