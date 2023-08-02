using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NPCLaser : MonoBehaviour
{
    [SerializeField]
    public Define.LaserLayer defaultLaserLayer;
    [SerializeField]
    private Define.LaserLayer hitLaserLayer;
    [Header("LaserProperty")]
    [SerializeField]
    private bool shoot = true;
    [SerializeField]
    private LaserColorData laserColorData;
    [SerializeField]
    private Define.LaserColor laserBaseColor;
    [SerializeField]
    private Define.LaserColor laserHitColor;
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

    private ILaserAction laserAction;
    private RaycastHit preHit;
    private Renderer curMat;
    private RaycastHit curHit;
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

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;


        SetColorOverLifeTime();
        ignoreLayerMask = GetLayerMask(defaultLaserLayer);
        actionLayerMask = GetLayerMask(hitLaserLayer);
        //SetIgnoreLayerMask();
        LaserExit();
    }

    private void Update()
    {
        if (!shoot)
        {
            return;
        }
        ShootLaser();
    }

    private void ShootLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit maxLengthHit;
        if(Physics.Raycast(ray, out maxLengthHit, maxLength, ignoreLayerMask))
        {
            float hitLength = Vector3.Distance(transform.position, maxLengthHit.point) + 1.0f;
            if (Physics.Raycast(ray, out curHit, hitLength, actionLayerMask))
            {
                if(preHit.collider != null && enter)
                {
                    if (preHit.collider.name != curHit.collider.name)
                    {
                        Debug.Log(preHit.collider.name + " Laser End Action");
                        EndLaserAction(preHit); 
                        enter = false;
                        preHit = curHit;
                    }
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
                if(enter)
                {
                    EndLaserAction(preHit);
                    enter = false;
                }
                CreateLaserLine(maxLengthHit);
            }
        }
        else
        {
            if(enter)
            {
                EndLaserAction(preHit);
                enter = false;
            }
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxLength);
            hitEffect.gameObject.transform.localPosition = transform.forward * maxLength;
        }
    }

    #region LaserSetting
    public LayerMask GetLayerMask(Define.LaserLayer laserLayer)
    {
        LayerMask mask = LayerMask.GetMask("Layer0"); // 기본 레이어 추가

        for (int i = 0; i <= 31; i++)
        {
            if (((int)laserLayer & (1 << i)) != 0)
                mask |= 1 << i;
        }

        return mask;
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

    private void SetColorOverLifeTime()
    {
        laserStartColor = laserStart.colorOverLifetime;
        laserEndColor = laserEnd.colorOverLifetime;
        glowStartColor = glowStart.colorOverLifetime;
        glowEndColor = glowEnd.colorOverLifetime;
    }
    public void LaserEnter()
    {
        var color = laserColorData.GetLaserColor(laserHitColor);
        curMat.material = color.laserMat;

        laserStartColor.color = color.particleColor;
        laserEndColor.color = color.particleColor;
        glowStartColor.color = color.glowBoxColor;
        glowEndColor.color = color.glowBoxColor;
    }
    
    public void LaserExit()
    {
        var color = laserColorData.GetLaserColor(laserBaseColor);
        curMat.material = color.laserMat;

        laserStartColor.color = color.particleColor;
        laserEndColor.color = color.particleColor;
        glowStartColor.color = color.glowBoxColor;
        glowEndColor.color = color.glowBoxColor;
    }
    #endregion

    #region LaserAction
    public void StartLaser()
    {
        this.gameObject.SetActive(true);
        enter = false;
        shoot = true;
    }

    public void StopLaser()
    {
        shoot = false;
        EndLaserAction(curHit);
        this.gameObject.SetActive(false);
    }

    public void StartLaserAction(RaycastHit hit)
    {
        if(hit.collider == null)
        {
            return;
        }
        laserAction = hit.collider.gameObject.GetComponent<ILaserAction>();
        if (laserAction == null)
        {
            LaserExit();
            Debug.Log(hit.collider.gameObject.name + " is no Assigned <ILaserAction>");
        }
        else
        {
            LaserEnter();
            laserAction.OnLaserHit();
        }
    }

    public void EndLaserAction(RaycastHit hit)
    {
        if(shoot)
        {
            LaserExit();
        }
        if (hit.collider == null)
        {
            return;
        }
        var preLaserAction = hit.collider.gameObject.GetComponent<ILaserAction>();
        if (preLaserAction != null)
        {
            preLaserAction.OffLaserHit();
        }
    }

    public void EndLaserAction(RaycastHit hit, bool connect)
    {
        if (connect)
        {
            LaserExit();
        }
        if (hit.collider == null)
        {
            return;
        }
        var preLaserAction = hit.collider.gameObject.GetComponent<ILaserAction>();
        if (preLaserAction != null)
        {
            preLaserAction.OffLaserHit();
        }
    }
    #endregion
}
