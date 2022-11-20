using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionDetectController : MonoBehaviour
{
    [SerializeField]
    private InteractionUiController interactionUi; 

    public Interaction CurFocusingnIteraction { get => curFocusingnIteraction;}

    private readonly string INTERACTION_TAG = "Interaction";
    private Interaction curFocusingnIteraction;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(INTERACTION_TAG)) 
        {
            interactionUi.Open(Camera.main.WorldToScreenPoint(other.transform.position));
            curFocusingnIteraction = other.GetComponent<Interaction>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(CurFocusingnIteraction == null) 
        {
            return;
        }

        if(other.CompareTag(INTERACTION_TAG)) 
        {
            if(CurFocusingnIteraction.gameObject == other.gameObject)
            {
                interactionUi.Close();
                curFocusingnIteraction = null;
            }
        }
    }

}
