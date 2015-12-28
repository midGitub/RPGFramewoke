using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LuaInterface;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static GameManager gameManager;
    public bool isLowDevice;
    public float npcRefreshTime = 0.05f;
    private string lastErrorLog,lastException;
    private bool initRes = true;

    public Text text;

    void Start() {
        DontDestroyOnLoad(gameObject);
        gameObject.AddComponent<CheckUpdate>();
        InitManager();
        InitObjPool();    
        if (SystemInfo.systemMemorySize < Constants.LIMIT_MEMORY_SIZE || SystemInfo.processorCount < Constants.PROCESSOR_COUNT)
        {
            isLowDevice = true;
            npcRefreshTime = 0.09f;
        }
    }

    private void InitManager() {
        ResourceManager.getInstance().Init();
        NetworkManager.getInstance().Init();
        UIManager.getInstance().Init();
        SoundManager.getInstance().Init();
        TimerManager.getInstance().Init();
        LuaManager.getInstance().Init();
        //TODO other
    }


    private void InitObjPool() {
        GameObject objPoll = new GameObject("ObjectPool");
        objPoll.AddComponent<ObjectPool>();
        DontDestroyOnLoad(objPoll);
    }

    void Update() {
        if (initRes && AssetBundleManager.AssetBundleManifestObject != null) {
            initRes = false;
            ResourceManager.getInstance().StartDownLoad();
            LuaManager.getInstance().LoadFile();
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
        #if UNITY_EDITOR
                return;
        #else
        switch (type)
        {
            case LogType.Error:
                if (!message.Equals(lastErrorLog))
                    StartCoroutine(doUploadLog(string.Format("{0}\n{1}", message, stacktrace)));
                lastErrorLog = message;
                break;
            case LogType.Exception:
                if(!message.Equals(lastException))
                    StartCoroutine(doUploadLog(string.Format("{0}\n{1}", message, stacktrace)));
                lastException = message;
                break;
        }
        #endif
    }

    IEnumerator doUploadLog(string log)
    {
        WWWForm form = new WWWForm();
        form.AddField("log", log);
        WWW www = new WWW(Server.LogUploadUrl, form);
        yield return www;
        if (www.error != null && www.isDone)
        {
            GLog.Log("uploadDeviceInfo error is :" + www.error);
        }
        yield return null;
    }



//-----------------------------------test--------------------------------------------
    public void HotUpdateResource() {
        AssetBundleService.getInstance().LoadAsset("prefabs/object/cube.unity3d", "cube", testPrefabs);
    }

    public void HotUpdateScript() {
        object[] results = LuaManager.getInstance().CallMethod("GameManager", "hello");
        text.text = results[0].ToString();
    }

    public void LoadTxt() {
        CSkillLoader skillLoader = CStaticDownload<CSkillLoader>.getInstance().GetStaticInfo(100);
        GLog.Log(skillLoader.GetSkillName());
    }

    public void TestObjectPoll() {
        GameObject capsule = ObjectPool.instance.GetObjByName("Prefabs/Capsule");
        GameObject cube = ObjectPool.instance.GetObjByName("Prefabs/Cube");
        int range1 = Random.Range(-4, 0);
        capsule.transform.position = new Vector3(range1, range1, range1);
        int range2 = Random.Range(0, 4);
        cube.transform.position = new Vector3(range2, range2, range2);
    }

    public void StartGame() {
        GameObject canvas=GameObject.Find("Canvas");
        canvas.transform.Find("panel_test").gameObject.SetActive(false);
        canvas.transform.Find("panel_game_loading").gameObject.SetActive(true);
        StartCoroutine(ShowLoading());
    }

    IEnumerator ShowLoading() {
        GameObject canvas = GameObject.Find("Canvas");
        yield return new WaitForSeconds(0.5f);
        canvas.transform.Find("panel_game_loading").gameObject.SetActive(false);
        canvas.transform.Find("panel_ui_login").gameObject.SetActive(true);
    }

    public void Login() { 
        
    }

    private void testPrefabs(string assetName,UnityEngine.Object obj)
    {
        GameObject go = Instantiate(obj as GameObject);
        go.transform.position = Vector3.zero;
    }

//-----------------------------------test--------------------------------------------
}
