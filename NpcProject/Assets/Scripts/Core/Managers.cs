using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static bool isQuit = false;
    static Managers instance; // ���ϼ��� ����ȴ�
    static Managers Instance { get {
            if (!isQuit) 
            {
                Init();
            }
            return instance; } } // ������ �Ŵ����� �����´�
    
    #region CoreManager
    private PoolManager pool = new PoolManager();
    private ResourceManager resource = new ResourceManager();
    private SceneManagerEx scene = new SceneManagerEx();
    private GameManager game = new GameManager();
    private UIManager ui = new UIManager();
    private TalkManager talk = new TalkManager();
    private CameraManager cam = new CameraManager();
    private KeywordManager keyword = new KeywordManager();
    private SoundManager sound = new SoundManager();
    private TimeManager time = new TimeManager();
    private EffectManager effect = new EffectManager();
    private DataManager data = new DataManager();
    public static KeywordManager Keyword { get => Instance.keyword; }
    public static CameraManager Camera { get => Instance.cam; }
    public static PoolManager Pool { get => Instance.pool; }
    public static GameManager Game { get => Instance.game; }
    public static ResourceManager Resource { get => Instance.resource; }
    public static SceneManagerEx Scene { get => Instance.scene;  }
    public static UIManager UI { get => Instance.ui; }
    public static TalkManager Talk { get => Instance.talk; }
    public static SoundManager Sound { get => Instance.sound; }
    public static TimeManager Time { get => Instance.time; }
    public static EffectManager Effect { get => Instance.effect; }
    public static DataManager Data { get => Instance.data; }
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
            instance.keyword.Init();
            instance.scene.Init();
            instance.sound.Init();
            instance.data.Init();
        }

    }
    public static void OnSceneLoad() 
    {
        Keyword.OnSceneLoaded();
        Game.OnSceneLoaded();
        Talk.OnSceneLoaded();
    }
    public static void Clear()
    {
        UI.Clear();
        Scene.Clear();
        Pool.Clear();
        Keyword.Clear();
        Sound.Clear();
    }
    private void OnApplicationQuit()
    {
        isQuit = true;
        //// DontDestroyOnLoad로 표시된 모든 게임 오브젝트를 찾아서 배열에 저장합니다.
        //GameObject[] dontDestroyObjects = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.transform.parent == null && obj.scene.name == null).ToArray();

        //// 배열에 저장된 모든 게임 오브젝트의 이름을 출력합니다.
        //foreach (GameObject obj in dontDestroyObjects)
        //{
        //    Debug.Log(obj.name);
        //    Destroy(obj);
        //}
    }
}
