using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LuaInterface;

public class GameManager : BaseLua
{
    public static GameManager gameManager;
    public LuaScriptMgr uluaMgr;
    public bool isLowDevice;
    public float npcRefreshTime = 0.05f;

    public Text text;

    private bool initRes = true;

    void Awake() {
        gameManager = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        if (Constants.IS_FIRST_INIT_GAME)
            Init();
        if (SystemInfo.systemMemorySize < Constants.LIMIT_MEMORY_SIZE || SystemInfo.processorCount < Constants.PROCESSOR_COUNT)
        {
            isLowDevice = true;
            npcRefreshTime = 0.09f;
        }
    }

    private void Init() {         
        gameObject.AddComponent<CheckUpdate>();
        gameObject.AddComponent<NetworkManager>();
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<SoundManager>();
        gameObject.AddComponent<TimerManager>();
        //TODO other
        uluaMgr = new LuaScriptMgr();
        uluaMgr.Start();
        Constants.IS_FIRST_INIT_GAME = false;
    }

    void Update() {
        if (AssetBundleManager.AssetBundleManifestObject != null) {
            if (initRes) {
                initRes = false;
                Debug.Log("load complete!");
            }
        }
            

    }

    private void OnEnable()
    {
        Application.logMessageReceived += UploadLog;
    }

    private void OnDisable() {
        Application.logMessageReceived -= UploadLog;
    }

    private void UploadLog(string message, string stacktrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                
                break;

            case LogType.Warning:
                
                break;

            case LogType.Log:
                
                break;

            case LogType.Exception:
                
                break;
        }
    }





//-----------------------------------test--------------------------------------------
    public void HotUpdateResource() {
        AssetBundleService.getInstance().LoadAsset("prefabs/object/cube.unity3d", "cube", testPrefabs);
    }

    public void HotUpdateScript() {
        string baseUrl = AssetBundleManager.BaseLocalURL.Replace("file://", "");
        uluaMgr.DoFile(baseUrl + "testlua.lua");
        object[] results = CallMethod("hello");
        text.text = results[0].ToString();
    }

    private void testPrefabs(UnityEngine.Object obj)
    {
        GameObject go = Instantiate(obj as GameObject);
        go.transform.position = Vector3.zero;
    }
}
