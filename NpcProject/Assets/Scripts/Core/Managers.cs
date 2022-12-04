using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{

    static Managers instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return instance; } } // 유일한 매니저를 갖고온다
    #region CoreManager
    private PoolManager pool = new PoolManager();
    private ResourceManager resource = new ResourceManager();
    private SceneManagerEx scene = new SceneManagerEx();

    public static PoolManager Pool { get => Instance.pool; }
    public static ResourceManager Resource { get => Instance.resource; }
    public static SceneManagerEx Scene { get => Instance.scene;  }
    #endregion

    void Start()
    {
        Init();
    }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();
           
            instance.pool.Init();
        }
    }

    public static void Clear()
    {
        Pool.Clear();
    }
}
