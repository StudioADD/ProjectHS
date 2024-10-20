using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static bool Initialized { get; set; } = false;

    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    #region Core
    private DataMgr _data = new DataMgr();
    private InputMgr _input = null; // Init에서 생성
    private PoolMgr _pool = new PoolMgr();
    private ResourceMgr _resource = new ResourceMgr();
    private SceneMgr _scene = new SceneMgr();
    private UIMgr _ui = new UIMgr();

    public static DataMgr Data { get { return Instance?._data; } }
    public static InputMgr Input { get { return Instance?._input; } }
    public static PoolMgr Pool { get { return Instance?._pool; } }
    public static ResourceMgr Resource { get { return Instance?._resource; } }
    public static SceneMgr Scene { get { return Instance?._scene; } }
    public static UIMgr UI { get { return Instance?._ui; } }
    #endregion

    public static void Init()
    {
        if (s_instance == null && Initialized == false)
        {
            Initialized = true;

            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            // 초기화
            s_instance = go.GetComponent<Managers>();
            s_instance._data.Init();
            s_instance._input = Instance._resource.Instantiate(PrefabPath.INPUTMANAGER_PATH, s_instance.transform).GetComponent<InputMgr>();
            s_instance._input.Init();
            s_instance._ui.Init();
        }
    }

    /// <summary>
    /// 씬 이동 시 호출
    /// </summary>
    public static void Clear()
    {
        Pool.Clear();
        Scene.Clear();
    }
}
