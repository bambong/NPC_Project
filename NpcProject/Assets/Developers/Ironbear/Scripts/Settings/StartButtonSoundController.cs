using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartButtonSoundController : MonoBehaviour 
{
    [SerializeField]
    private StartPausePanelController startPausePanelController;

    private Coroutine pointerCo;
 
    public void StartPointerUpCheck() 
    {
        if (startPausePanelController.IsTransition) 
        {
            return;
        }
        if (pointerCo != null || !gameObject.activeInHierarchy)
        {
            return;
        }
        pointerCo = StartCoroutine(PointerUpCheck());
    }
    IEnumerator PointerUpCheck() 
    {
        while (true) 
        {
            if (Input.GetMouseButtonUp(0))
            {
                Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
                pointerCo = null;
                yield break;
            }
            yield return null;
        }
        
    }
    private void OnDisable()
    {
        if (pointerCo != null)
        {
            StopCoroutine(pointerCo);
            pointerCo = null;
        }
    }


}
