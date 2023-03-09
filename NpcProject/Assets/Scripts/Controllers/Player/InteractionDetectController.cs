using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractionDetectController : MonoBehaviour
{
    private InteractionUiController interactionUi;
    
    private IInteraction curIteraction;
    public IInteraction CurIteraction { get => curIteraction; }

    private readonly string INTERACTION_TAG = "Interaction";

    public void Init()
    {
        interactionUi = Managers.UI.MakeWorldSpaceUI<InteractionUiController>(Managers.Scene.CurrentScene.transform,"InteractionUI");
     
    }
    public void SwitchDebugMod(bool isOn)
    {
        if (isOn) 
        {
            if(curIteraction != null) 
            {
                interactionUi.Close();
                curIteraction = null;
            }
            gameObject.SetActive(false);
        }
        else 
        {
            gameObject.SetActive(true);
        }
    }

    public void Interaction()
    {
        if(Managers.Game.IsDebugMod)
        {
            return; 
        }

        if(curIteraction != null)
        {
            Managers.Game.Player.SetStateInteraction();
            curIteraction.OnInteraction();
        }

    }
    public void InteractionUiEnable() => interactionUi.Open();
    public void InteractionUiDisable() => interactionUi.Close();


    private void SetInteraction(GameObject go) 
    {
        var interaction = go.GetComponent<IInteraction>();

        if (interaction != null)
        {
            interactionUi.Open(go.transform);
            curIteraction = interaction;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (Managers.Game.IsDebugMod) 
        {
            return;
        }
        SetInteraction(other.gameObject);
    }
   

    private void OnTriggerExit(Collider other)
    {
        if (Managers.Game.IsDebugMod)
        {
            return;
        }
    
        if (CurIteraction == null)
        {
            return;
        }
        var interaction = other.GetComponent<IInteraction>();

        if (interaction != null && interaction == CurIteraction)
        {
            interactionUi.Close();
            curIteraction = null;
        }
    }

}
