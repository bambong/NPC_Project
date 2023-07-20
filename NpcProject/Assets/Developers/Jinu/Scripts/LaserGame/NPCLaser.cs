using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NPCLaser : MonoBehaviour
{    
    [SerializeField]
    private string[] correctTag;
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

    private bool enter = false;
    private int layerMask;

    private void Start()
    {
        tagName = new StringBuilder();

        hitEffect = baseLaserEnd;

        curMat = GetComponent<Renderer>();
        originMat = curMat.material;

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;

        layerMask = (-1) - (1 << LayerMask.NameToLayer("InteractionDetector"));
    }

    private void Update()
    {
         ShootLaser();
    }

    private void ShootLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxLength, layerMask))
        { 
            if(CheckTag(hit))
            {                
                if(!enter)
                {
                    LaserEnter();
                    StartLaserAction(hit);
                    enter = true;
                }                
            }
            else
            {
                if(enter)
                {
                    LaserExit();
                    EndLaserAction();
                    enter = false;
                }
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
                EndLaserAction();
                enter = false;
            }            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxLength);
            hitEffect.gameObject.transform.localPosition = transform.forward * maxLength;
        }
    }
    public bool CheckName(string name)
    {
        if (tagName.ToString() != name)
        {
            tagName.Clear();
            tagName.Append(name);
            return false;

        }
        else
        {
            return true;
        }
    }

    public bool CheckTag(RaycastHit hit)
    {
        for(int i = 0; i < correctTag.Length; i++)
        {
            if (hit.collider.CompareTag(correctTag[i]))
            {
                return true;
            }
        }
        return false;
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

    public void EndLaserAction()
    {
        if (laserAction != null)
        {
            laserAction.OffLaserHit();
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
