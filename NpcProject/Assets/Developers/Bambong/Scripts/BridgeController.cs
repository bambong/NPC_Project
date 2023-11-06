using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField]
    private List<BridgePartController> bridgePartControllers;

    [SerializeField]
    private float timeInterval;
    private bool isSuccess = false;

    [SerializeField]
    private string hideEvnetGuid;

    private void Start()
    {        
        if(Managers.Data.IsClearEvent(hideEvnetGuid))
        {
            Hide();
        }
    }

    [ContextMenu("SUCCESS")]
    public void OnSuccess() 
    {
        //if (isSuccess)
        //{
        //    return;
        //}
        isSuccess = true;
        StartCoroutine(Success());
    }
    [ContextMenu("HIDE BRIDGE")]
    public void Hide()
    {
        for(int i = 0; i < bridgePartControllers.Count; ++i)
        {
            bridgePartControllers[i].Hide();
        }
    }
    [ContextMenu("HIDE EVENT")]
    public void HideEvent()
    {
        StartCoroutine(HideBridges());
    }

    IEnumerator Success() 
    {
        var time =  new WaitForSeconds(timeInterval);
        for (int i =0; i < bridgePartControllers.Count; ++i) 
        {
            bridgePartControllers[i].OnSuccess();
            yield return time;
        }
    }

    IEnumerator HideBridges()
    {
        var time = new WaitForSeconds(timeInterval);
        for (int i = 0; i < bridgePartControllers.Count; ++i)
        {
            bridgePartControllers[i].HideEvent();
            yield return time;
        }
    }

}
