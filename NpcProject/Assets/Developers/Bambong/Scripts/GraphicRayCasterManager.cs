using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GraphicRayCasterManager : GameObjectSingletonDestroy<GraphicRayCasterManager>, IInit
{
    [SerializeField]
    private GraphicRaycaster graphicRaycaster;
    private List<RaycastResult> raycastResults =new List<RaycastResult>();


    public void Init()
    {

    }
    public List<RaycastResult> GetRaycastList(PointerEventData pointerEventData) 
    {
        raycastResults.Clear();
        graphicRaycaster.Raycast(pointerEventData,raycastResults);
        return raycastResults;
    }

}
