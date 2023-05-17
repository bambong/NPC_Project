using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
    [SerializeField]
    private GameObject notIncludeObject;
        
    [SerializeField]
    private GameObject laserObject; // Laser

    private float distance;

    private bool clear = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name != notIncludeObject.gameObject.name)
        {
            distance = Vector3.Distance(notIncludeObject.transform.position, other.gameObject.transform.position);
            Debug.Log(distance);
            this.transform.localPosition -= new Vector3(0, 0, distance - 15f);
            this.transform.localScale -= new Vector3(0, distance - 15f, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.transform.localPosition += new Vector3(0, 0, distance - 15f);
        this.transform.localScale += new Vector3(0, distance - 15f, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("CollisionEnter " + collision.gameObject.name);
    }


}
