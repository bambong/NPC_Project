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
    private GameManager game = new GameManager();
    private UIManager ui = new UIManager();
    private TalkManager talk = new TalkManager();
    private CameraManager cam = new CameraManager();
    private KeywordManager keyword = new KeywordManager();

    public static KeywordManager Keyword { get => Instance.keyword; }
    public static CameraManager Camera { get => Instance.cam; }
    public static PoolManager Pool { get => Instance.pool; }
    public static GameManager Game { get => Instance.game; }
    public static ResourceManager Resource { get => Instance.resource; }
    public static SceneManagerEx Scene { get => Instance.scene;  }
    public static UIManager UI { get => Instance.ui; }
    public static TalkManager Talk { get => Instance.talk; }
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
           
            instance.talk.Init();
            instance.game.Init();
            instance.pool.Init();
            instance.cam.Init();
        }

    }


    public static void Clear()
    {
        UI.Clear();
        Scene.Clear();
        Pool.Clear();
    }
}
