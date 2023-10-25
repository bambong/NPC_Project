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
    [ContextMenu("SUCCESS")]
    public void OnSuccess() 
    {
        if (isSuccess)
        {
            return;
        }
        isSuccess = true;
        StartCoroutine(Success());
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

}
