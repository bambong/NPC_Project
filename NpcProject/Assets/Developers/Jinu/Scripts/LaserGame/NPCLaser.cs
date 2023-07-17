using System.Collections;
using System.Collections.Generic;
using System;
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
    private GameObject hitEffect;
    [SerializeField]
    private ParticleSystem laserStart;
    [SerializeField]
    private ParticleSystem glowStart;
    [SerializeField]
    private ParticleSystem laserEnd;
    [SerializeField]
    private ParticleSystem glowEnd;
    [Header("Hit Effect")]
    [SerializeField]
    private Gradient baseColor;
    [SerializeField]
    private Gradient baseGlowColor;
    [SerializeField]
    private Gradient hitColor;
    [SerializeField]
    private Gradient hitGlowColor;
    [SerializeField]
    private Material hitMat;


    private ILaserAction laserAction;
    private RaycastHit preHit;
    private Renderer curMat;
    private Material originMat;
    private ParticleSystem.ColorOverLifetimeModule laserStartColor;
    private ParticleSystem.ColorOverLifetimeModule laserEndColor;
    private ParticleSystem.ColorOverLifetimeModule glowStartColor;
    private ParticleSystem.ColorOverLifetimeModule glowEndColor;

    private bool enter = false;
    private int ignoreLayerMask = 0;
    private int actionLayerMask = 0;

    private void Start()
    {
        curMat = GetComponent<Renderer>();
        originMat = curMat.material;

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;


        SetColorOverLifeTime();
        SetLayerMask();
        SetIgnoreLayerMask();
    }

    private void Update()
    {
         ShootLaser();
    }

    private void ShootLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit curHit;
        RaycastHit maxLengthHit;
        if(Physics.Raycast(ray, out maxLengthHit, maxLength, ignoreLayerMask))
        {
            if (Physics.Raycast(ray, out curHit, maxLength, actionLayerMask))
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
                catch (NullReferenceException)
                {
                    Debug.Log("preHit Init");
                    preHit = curHit;
                }

                if (!enter)
                {
                    StartLaserAction(curHit);
                    preHit = curHit;
                    enter = true;
                }
                CreateLaserLine(curHit);
            }
            else
            {
                LaserExit();
                EndLaserAction(preHit);
                CreateLaserLine(maxLengthHit);
            }
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

        laserStartColor.color = hitColor;
        laserEndColor.color = hitColor;
        glowStartColor.color = hitGlowColor;
        glowEndColor.color = hitGlowColor;
    }
    
    public void LaserExit()
    {
        curMat.material = originMat;

        laserStartColor.color = baseColor;
        laserEndColor.color = baseColor;
        glowStartColor.color = baseGlowColor;
        glowEndColor.color = baseGlowColor;
    }

    public void StartLaserAction(RaycastHit hit)
    {
        if(hit.collider == null)
        {
            return;
        }
        laserAction = hit.collider.gameObject.GetComponent<ILaserAction>();
        if (laserAction != null)
        {
            LaserEnter();
            laserAction.OnLaserHit();
        }
        else
        {
            Debug.Log(hit.collider.gameObject.name + " is no Assigned <ILaserAction>");
        }
    }

    public void EndLaserAction(RaycastHit hit)
    {
        if(hit.collider == null)
        {
            return;
        }
        var preLaserAction = hit.collider.gameObject.GetComponent<ILaserAction>();
        if (preLaserAction != null)
        {
            preLaserAction.OffLaserHit();
        } 
    }
    private void SetColorOverLifeTime()
    {
        laserStartColor = laserStart.colorOverLifetime;
        laserEndColor = laserEnd.colorOverLifetime;
        glowStartColor = glowStart.colorOverLifetime;
        glowEndColor = glowEnd.colorOverLifetime;
    }

    public void SetLayerMask()
    {
        for (int i = 0; i < layerName.Length; i++)
        {
            actionLayerMask |= 1 << LayerMask.NameToLayer(layerName[i]);
        }
    }

    public void SetIgnoreLayerMask()
    {
        ignoreLayerMask = ~(1 << LayerMask.NameToLayer("InteractionDetector"));
    }

    public void CreateLaserLine(RaycastHit hit)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.point);
        hitEffect.gameObject.transform.position = hit.point;
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
