using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;
      
    private GameObject root;

    public GameObject Root
    {
        get
        { 
            if (root == null) 
            {
                root = GameObject.Find("@UI_Root");
                if (root != null)
                {
                    return root;
                }

                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }
    public T MakeSceneUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/{name}");
        if (parent != null)
            go.transform.SetParent(parent);
        
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var temp = Util.GetOrAddComponent<T>(go);
        temp.Init();
        return temp;
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
        if (parent != null)
        {
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
        }

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        var temp = Util.GetOrAddComponent<T>(go);
        temp.Init();
        return temp;
    }

    public T MakeCameraSpaceUI<T>(float distance ,Transform parent = null, string name = null ) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/CameraSpace/{name}");
        if (parent != null)
        {
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
        }

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = distance;
        var temp = Util.GetOrAddComponent<T>(go);
        temp.Init();
        return temp;
    }
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}", parent);
        if (parent != null) 
        {
            //go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
        }

        var result = Util.FindChild<T>(go);
        if(result != null) 
        {
            result.Init();
            return result;
        }
        var temp = Util.GetOrAddComponent<T>(go);
        temp.Init();

        return temp;
    }

    public void Clear()
    {

    }
}
