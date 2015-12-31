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

    void Start() {
        DontDestroyOnLoad(gameObject);
        gameObject.AddComponent<CheckUpdate>();
        InitManager();  
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
        ObjectManager.getInstance().Init();
        //TODO other
    }

    void Update() {
        if (initRes && AssetBundleManager.AssetBundleManifestObject != null) {
            initRes = false;
            ResourceManager.getInstance().StartDownLoad();
            LuaManager.getInstance().LoadFile();
        }
        UIManager.getInstance().Update();
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
        www.Dispose();
        yield return null;
    }

    void OnDestroy()
    {
        LuaManager.getInstance().OnDestroy();
        ResourceManager.getInstance().OnDestroy();
    }


//-----------------------------------test--------------------------------------------
    private bool isOpen = true;

    void OnGUI()
    {
        if (isOpen)
        if (GUI.Button(new Rect(200, 200 + 100, 100, 60), "开始"))
        {
            //SingletonObject<LoadingMediator>.getInstance().Open();
            isOpen = false;
            //SceneXmlObj xmlObj = XmlTools.ParseXmlByPath(Application.dataPath + "/StaticDatas/tesMecanim.xml");
            //GLog.Log("--------------------------"+xmlObj.SceneName);

            XmlTools.test();
        }
    }

//-----------------------------------test--------------------------------------------
}
