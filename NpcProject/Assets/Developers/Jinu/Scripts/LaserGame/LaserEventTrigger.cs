using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEventTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private Material hitmat;
    [SerializeField]
    private LaserColorController laserColorController;
    [SerializeField]
    private Color hitcolor;

    private Material curmat;
    private Material originmat;
    private bool first = true;
    

    public virtual void OnEventTrigger(Collider other)
    {
        if(first)
        {
            curmat = target.GetComponent<Renderer>().material;            

            originmat = curmat;            
            first = false;
        }
        laserColorController.SetLaserColor(hitcolor);
        curmat = hitmat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == target.name)
        {
            OnEventTrigger(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == target.name)
        {
            curmat = originmat;
        }
    }
}
