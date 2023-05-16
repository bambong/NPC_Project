using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMatchCheckController : DebugModEffectController
{

    private float CLEAR_DISTANCE = 1f;
    private float CLEAR_ROTATE = 20f;
    private Transform curTarget;
    [SerializeField]
    private Color clearColor;
    private Color originColor;
    private Renderer renderer;



    public override void EnterDebugMod()
    {
        renderer.enabled = true;
    }

    public override void ExitDebugMod()
    {
        renderer.enabled = false;
    }

    public override void Init()
    {
        base.Init();
        renderer = GetComponent<Renderer>();
        renderer.enabled = false;
        originColor = renderer.material.GetColor("_Main_Color");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interaction")) 
        {
            curTarget = other.transform;
        }   
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Interaction"))
        {
            curTarget = other.transform;
            if(IsClear()) 
            {
                renderer.material.SetColor("_Main_Color",clearColor);
            }
            else 
            {
                renderer.material.SetColor("_Main_Color",originColor);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interaction") && other.transform == curTarget)
        {
            renderer.material.SetColor("_Main_Color",originColor);
            curTarget = null;
        }
    }
    public bool IsClear() 
    {
        if(curTarget == null) 
        {
            return false;
        }

        var dis = (curTarget.transform.position - transform.position).magnitude;
        if ( dis > CLEAR_DISTANCE) 
        {
            return false;
        }

        var rot = Quaternion.Angle(transform.rotation, curTarget.rotation);
        if( rot > CLEAR_ROTATE) 
        {
            return false;
        }

        return true;
    }

    public void Clear() 
    {
        curTarget.transform.position = transform.position;
        curTarget.transform.rotation = transform.rotation;
        gameObject.SetActive(false);
    }


}
