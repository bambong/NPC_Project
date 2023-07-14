using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NPCLaser : MonoBehaviour
{    
    [SerializeField]
    private string[] layerName;
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

    private ILaserAction laserAction;
    private GameObject hitEffect;
    private Renderer curMat;
    private Material originMat;
    private StringBuilder tagName;
    private RaycastHit preHit;

    private bool enter = false;
    private int layerMask = 0;

    private void Start()
    {
        tagName = new StringBuilder();

        hitEffect = baseLaserEnd;

        curMat = GetComponent<Renderer>();
        originMat = curMat.material;

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;

        SetLayerMask();
    }

    private void Update()
    {
         ShootLaser();
    }

    private void ShootLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit curHit;

        if (Physics.Raycast(ray, out curHit, maxLength, layerMask))
        {
            try
            {
                if (preHit.collider.name != curHit.collider.name)
                {
                    LaserExit();
                    EndLaserAction(preHit);
                    enter = false;
                    preHit = curHit;
                }
            }
            catch(NullReferenceException)
            {
                Debug.Log("preHit Init");
                preHit = curHit;
            }

            if(!enter)
            {
                preHit = curHit;
                LaserEnter();
                StartLaserAction(curHit);
                enter = true;
            }

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, curHit.point);
            hitEffect.gameObject.transform.position = curHit.point;
            
        }
        else
        {
            if(enter)
            {
                LaserExit();
                EndLaserAction(preHit);
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

    public void StartLaserAction(RaycastHit hit)
    {
        laserAction = hit.collider.gameObject.GetComponent<ILaserAction>();
        if (laserAction != null)
        {
            laserAction.OnLaserHit();
        }
        else
            Debug.Log(hit.collider.gameObject.name + " is no Assigned <ILaserAction>");
        //try
        //{
        //    laserAction = hit.collider.gameObject.GetComponent<ILaserAction>();
        //    laserAction.OnLaserHit();
        //}
        //catch(NullReferenceException)
        //{
        //    Debug.Log(hit.collider.gameObject.name + " is no Assigned <ILaserAction>");
        //}
    }

    public void EndLaserAction(RaycastHit hit)
    {
        var preLaserAction = hit.collider.gameObject.GetComponent<ILaserAction>();
        if (preLaserAction != null)
        {
            preLaserAction.OffLaserHit();
        } 
    }

    public void SetLayerMask()
    {
        preHit = new RaycastHit();
        for (int i = 0; i < layerName.Length; i++)
        {
            layerMask |= 1 << LayerMask.NameToLayer(layerName[i]);
        }
    }

    //public void SetHitLaserColor(ParticleSystem baseParticle, Color hitcolor)
    //{
    //    ParticleSystem.ColorOverLifetimeModule colorOverLifetime = baseParticle.colorOverLifetime;
    //    Gradient gradient = colorOverLifetime.color.gradient;

    //    GradientColorKey[] colorKeys = blueParticle.colorOverLifetime.color.gradient.colorKeys;

    //    gradient.colorKeys = colorKeys;

    //    colorOverLifetime.color = gradient;
    //}
}
