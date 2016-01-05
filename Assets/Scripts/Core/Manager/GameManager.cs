using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LuaInterface;
using System.IO;
using System.Collections.Generic;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static GameManager gameManager;
    public bool isLowDevice;
    public float npcRefreshTime = 0.05f;
    private string lastErrorLog,lastException;
    private bool initRes = true;

    void Start() {
        DontDestroyOnLoad(gameObject);
        //暂时不检查更新
        //gameObject.AddComponent<CheckUpdate>();
        AssetBundleManager.SetLocalAssetBundleDir();
        gameObject.AddComponent<AssetBundleService>();

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
        //LuaManager.getInstance().Init();
        ObjectManager.getInstance().Init();
        //TODO other
    }

    void Update() {
        if (initRes && AssetBundleManager.AssetBundleManifestObject != null) {
            initRes = false;
            ResourceManager.getInstance().StartDownLoad();
            //LuaManager.getInstance().LoadFile();
        }
        UIManager.getInstance().Update();
    }

    IEnumerator CopyFilesToPersistent() {
        string copyTo = Util.ToPath;
        string copyFrom = Util.FromPath;
        Debug.Log("copyTo----" + copyTo + "--copyFrom----" + copyFrom);
        FindFiles(copyFrom);
        Debug.Log("dirPaths-------" + dirPaths.Count + "----filePaths---" + filePaths.Count);
        string tempPath=null;
        foreach (string path in dirPaths)
        {
            tempPath = copyTo + Util.CutString(path, "StreamingAssets");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
        }
        int size=filePaths.Count;
        for (int i = 0; i < size; i++) {
            tempPath = copyTo + Util.CutString(filePaths[i], "StreamingAssets");
            if (Application.isMobilePlatform)
            {
                WWW www = new WWW(filePaths[i]);
                yield return www;
                if (www.isDone && www.error == null)
                {
                    File.WriteAllBytes(tempPath, www.bytes);
                }
                yield return 0;
            }
            else
                File.Copy(filePaths[i], tempPath, true);
            yield return new WaitForEndOfFrame();
            SingletonObject<LoadingMediator>.getInstance().Progress = (i + 1) / (float)size;
        }
        yield return null;
        dirPaths.Clear();
        filePaths.Clear();
    }

    private List<string> dirPaths = new List<string>();
    private List<string> filePaths = new List<string>();

    private void FindFiles(string path) {
        if (File.Exists(path))
        {
            filePaths.Add(path);
        }
        else if (Directory.Exists(path))
        {
            dirPaths.Add(path);
            string[] dirs=Directory.GetDirectories(path);
            foreach (string dir in dirs)
                FindFiles(dir);
            string[] files=Directory.GetFiles(path);
            foreach (string file in files)
                FindFiles(file);
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
            SingletonObject<TestMediator>.getInstance().Open();
            //StartCoroutine(CopyFilesToPersistent());
            //SingletonObject<LoadingMediator>.getInstance().Open();
            isOpen = false;
        }
    }

//-----------------------------------test--------------------------------------------
}
