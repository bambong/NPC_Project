using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractionDetectController : MonoBehaviour
{
    [SerializeField]
    private InteractionUiController interactionUi;
    [SerializeField]
    private EffectController Effect;

    public Interaction CurFocusingnIteraction { get => curFocusingnIteraction;}

    private readonly string INTERACTION_TAG = "Interaction";
    private Interaction curFocusingnIteraction;


    public void Interaction() 
    {
        if(curFocusingnIteraction != null)
        {
            curFocusingnIteraction.OnInteraction();
            GameSceneManager.Instacne.Player.SetStateInteraction();
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Test")
            {
                SceneManager.LoadScene("Debug");
                Effect.LoadNextLevel();
            }

            if (SceneManager.GetActiveScene().name == "Debug")
            {
                SceneManager.LoadScene("Test");
            }
        }
    }
    public void InteractionUiEnable() => interactionUi.Open();
    public void InteractionUiDisable() => interactionUi.Close();

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
