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
    private bool copyDone = false;
    private eGameState currentState;//当前状态

    void Start() {
        DontDestroyOnLoad(gameObject);
        //暂时不检查更新
        //gameObject.AddComponent<CheckUpdate>();
        AssetBundleManager.SetLocalAssetBundleDir();
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
        if (copyDone)
        {          
            gameObject.AddComponent<AssetBundleService>();
            copyDone=false;
        }
        if(initRes && AssetBundleManager.AssetBundleManifestObject != null){
            initRes = false;
            ResourceManager.getInstance().StartDownLoad();
            LuaManager.getInstance().Init();
            LuaManager.getInstance().LoadFile();
        }
        UIManager.getInstance().Update();
        LoadingManager.getInstance().Update();
    }

    public void SetGameState(eGameState state)
    {
        currentState = state;
        switch(state){
            case eGameState.Loading:

                break;

            case eGameState.Login:
                SingletonObject<TestMediator>.getInstance().Open();
                break;
        }
    }

    IEnumerator CopyToPersistent() {
        string copyTo = Util.ToPath;
        if (!Directory.Exists(copyTo))
            Directory.CreateDirectory(copyTo);    
        string copyFrom = Util.FromPath;
        string fileListFrom = copyFrom+"/files.txt";
        string fileListTo = copyTo + "/files.txt";
        if (File.Exists(fileListTo)) File.Delete(fileListTo);
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(fileListFrom);
            yield return www;
            if (www.isDone && www.error==null)
            {
                File.WriteAllBytes(fileListTo, www.bytes);
            }
            www.Dispose();
            yield return 0;
        }
        else
            File.Copy(fileListFrom, fileListTo, true);
        yield return new WaitForEndOfFrame();
        string[] lines=File.ReadAllLines(fileListTo);
        int len=lines.Length;
        string fileFrom = null;
        string fileTo = null;
        for (int i = 0; i < len; i++)
        {
            fileFrom = Util.FromPath + "/" + lines[i];
            fileTo = Util.ToPath + "/" + lines[i];
            string dir=Path.GetDirectoryName(fileTo);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(fileFrom);
                yield return www;
                if (www.isDone && www.error == null)
                {
                    File.WriteAllBytes(fileTo, www.bytes);
                }
                www.Dispose();
                yield return 0;
            }
            else File.Copy(fileFrom, fileTo, true);
            yield return new WaitForEndOfFrame();
            SingletonObject<LoadingMediator>.getInstance().Progress = (i + 1) / (float)len;
        }
        copyDone = true;
        yield return null;
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
            //StartCoroutine(CopyFilesToPersistent());
            StartCoroutine(CopyToPersistent());
            SingletonObject<LoadingMediator>.getInstance().Open();
            isOpen = false;
        }
    }

    #region 根据目录路径读取该目录下所有文件夹及文件夹，并将其复制到Persistent路径(Android平台不可用)
    private List<string> dirPaths;
    private List<string> filePaths;

    IEnumerator CopyFilesToPersistent()
    {
        string copyTo = Util.ToPath;
        string copyFrom = Util.FromPath;
        Debug.Log("copyTo----" + copyTo + "--copyFrom----" + copyFrom);
        dirPaths = new List<string>();
        filePaths = new List<string>();
        FindFiles(copyFrom);
        Debug.Log("dirPaths-------" + dirPaths.Count + "----filePaths---" + filePaths.Count);
        string tempPath = null;
        foreach (string path in dirPaths)
        {
            tempPath = copyTo + Util.CutString(path, "StreamingAssets");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
        }
        int size = filePaths.Count;
        for (int i = 0; i < size; i++)
        {
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

    private void FindFiles(string path)
    {
        if (File.Exists(path))
        {
            filePaths.Add(path);
        }
        else if (Directory.Exists(path))
        {
            dirPaths.Add(path);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
                FindFiles(dir);
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
                FindFiles(file);
        }
    }
    #endregion

    //-----------------------------------test--------------------------------------------
}
