using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractionDetectController : MonoBehaviour
{
    private InteractionUiController interactionUi;
    private InteractionUiController keyworInteractionUi;
    
    private IInteraction curIteraction;
    private KeywordEntity curKeywordIteraction;

    public IInteraction CurIteraction { get => curIteraction; }
    public KeywordEntity CurKeywordIteraction { get => curKeywordIteraction; }
 
    private readonly string INTERACTION_TAG = "Interaction";

    public void Init()
    {
        interactionUi = Managers.UI.MakeWorldSpaceUI<InteractionUiController>(Managers.Scene.CurrentScene.transform,"InteractionUI");
        keyworInteractionUi = Managers.UI.MakeWorldSpaceUI<InteractionUiController>(Managers.Scene.CurrentScene.transform, "KeywordInteractionUI");
    }
    public void SwitchDebugMod(bool isOn)
    {

        if (isOn) 
        {
            if(curIteraction != null) 
            {
                interactionUi.Close();
                SetKeywordInteraction(curIteraction.Go);
                curIteraction = null;
            }
        }
        else 
        {
            if(curKeywordIteraction != null) 
            {
                keyworInteractionUi.Close();
                SetInteraction(curKeywordIteraction.gameObject);
                curKeywordIteraction = null;
            }  
        
        }

    }

    public void Interaction() 
    {
        if(curIteraction != null)
        {
            curIteraction.OnInteraction();
            Managers.Game.Player.SetStateInteraction();
        }

    }
    public void InteractionUiEnable() => interactionUi.Open();
    public void InteractionUiDisable() => interactionUi.Close();

    private void SetKeywordInteraction(GameObject go)
    {
        var keyword = go.GetComponent<KeywordEntity>();

        if (keyword != null)
        {
            curKeywordIteraction = keyword;
            keyworInteractionUi.Open(go.transform);
        }

    }
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
            SetKeywordInteraction(other.gameObject);
        }
        else 
        {
            SetInteraction(other.gameObject);
        }
    }
   

    private void OnTriggerExit(Collider other)
    {
        if (Managers.Game.IsDebugMod)
        {
            if(CurKeywordIteraction == null) 
            {
                return;
            }
            var keyword = other.GetComponent<KeywordEntity>();

            if (keyword != null&& keyword == CurKeywordIteraction)
            {
                keyworInteractionUi.Close();
                curKeywordIteraction = null;
            }
        }
        else
        {
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

}
