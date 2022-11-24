using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInit 
{
    void Init();
}

public abstract class GameObjectSingleton<T> : MonoBehaviour where T : MonoBehaviour , IInit
{
    public static T Instance 
    {
        get 
        {
            if(instance == null) 
            {
                var objs = FindObjectsOfType(typeof(T));

                if(objs.Length == 1) 
                {
                    instance = (T) objs[0];
                    instance.Init();
                }
                else if(objs.Length > 1) 
                {
                    Debug.LogError($"{typeof(T).Name} Singleton Instance 가 1개 보다 더 많습니다.");
                }

                if(instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    instance.Init();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    
                }
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    
    private static T instance;
 

   
}
