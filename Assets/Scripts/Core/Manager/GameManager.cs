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
            InitManager();
        ResourceManager.getInstance().init();
        if (SystemInfo.systemMemorySize < Constants.LIMIT_MEMORY_SIZE || SystemInfo.processorCount < Constants.PROCESSOR_COUNT)
        {
            isLowDevice = true;
            npcRefreshTime = 0.09f;
        }
    }

    private void InitManager() {         
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
                ResourceManager.getInstance().StartDownLoad();
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
        //AssetBundleService.getInstance().LoadAsset("prefabs/object/cube.unity3d", "cube", testPrefabs);
        CSkillLoader skillLoader=CStaticDownload<CSkillLoader>.getInstance().GetStaticInfo(100);
        GLog.Log(skillLoader.GetSkillName());
    }

    public void HotUpdateScript() {
        string baseUrl = AssetBundleManager.BaseLocalURL.Replace("file://", "");
        uluaMgr.DoFile(baseUrl + "testlua.lua");
        object[] results = CallMethod("hello");
        text.text = results[0].ToString();
    }

    private void testPrefabs(string assetName,UnityEngine.Object obj)
    {
        GameObject go = Instantiate(obj as GameObject);
        go.transform.position = Vector3.zero;
    }
}
