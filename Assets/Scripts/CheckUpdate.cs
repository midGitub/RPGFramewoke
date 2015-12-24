using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using ICSharpCode;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

public class CheckUpdate : MonoBehaviour {
    public delegate void HandleFinishDownload(WWW www);  

    public static readonly string VERSION_FILE = "version.txt";
    public static readonly string PATCH_FILE = "patch.zip";

    private List<string> localContent;
    private List<string> remoteContent;
    private List<string> updateFiles;
    private bool needUpdate = false;

    void Awake() {
        AssetBundleManager.SetLocalAssetBundleDir();
    }

    void Start()
    {       
        //初始化  
        localContent = new List<string>();
        remoteContent = new List<string>();
        //加载本地version配置  
        StartCoroutine(DownLoad(AssetBundleManager.BaseLocalURL + VERSION_FILE, delegate(WWW localVersion)
        {
            //解析本地version文件
            ParseVersionFile(localVersion.text, localContent);
            //加载服务端version配置  
            StartCoroutine(DownLoad(Server.RemoteAssetBundleUrl + VERSION_FILE, delegate(WWW serverVersion)
            {
                //解析服务端vertion文件
                ParseVersionFile(serverVersion.text, remoteContent);
                //计算出需要重新加载的资源  
                CompareVersion();
                //加载需要更新的资源  
                DownLoadRes();
            }));
        }));
    }

    //依次加载需要更新的资源  
    private void DownLoadRes()
    {
        if (needUpdate)
            StartCoroutine(CopyFiles());
        else
            gameObject.AddComponent<AssetBundleService>();
    }

    IEnumerator CopyFiles() {
        WWW patchWWW = new WWW(Server.RemoteAssetBundleUrl + PATCH_FILE);
        yield return patchWWW;
        if(patchWWW.isDone && patchWWW.error==null){
            if (SaveDataToLocal(PATCH_FILE, patchWWW.bytes))
            {
                ApplyPatch(PATCH_FILE);
                //解压完之后删除
                File.Delete(AssetBundleManager.BaseLocalURL.Replace("file://", "")+PATCH_FILE);
                patchWWW.Dispose();  
                //再复制version文件
                WWW versionWWW = new WWW(Server.RemoteAssetBundleUrl + VERSION_FILE);
                yield return versionWWW;
                if (versionWWW.isDone && versionWWW.error == null)
                {
                    SaveDataToLocal(VERSION_FILE, versionWWW.bytes);
                    versionWWW.Dispose();
                    gameObject.AddComponent<AssetBundleService>();
                }
            } 
        }            
    }

    //比较版本
    private void CompareVersion()
    {
        int locaVersion=int.Parse(localContent[0].Split('=')[1]);
        int remoteVersion = int.Parse(remoteContent[0].Split('=')[1]);
        if (locaVersion != remoteVersion)
        {
            updateFiles = new List<string>();
            for (int i = 1; i < remoteContent.Count;i++ )
            {
                updateFiles.Add(remoteContent[i]);
            }
            //本次有更新，同时更新本地的version.txt  
            needUpdate = true;
        }      
    }

    private void ParseVersionFile(string content, List<string> dict)
    {
        if (content == null || content.Length == 0)
        {
            return;
        }
        string[] items = content.Split(new char[] {'\n'});
        foreach (string item in items)
        {
            dict.Add(item);
        }

    }

    private IEnumerator DownLoad(string url, HandleFinishDownload finishFun)
    {
        WWW www = new WWW(url);
        yield return www;
        if (finishFun != null)
        {
            finishFun(www);
        }
        www.Dispose();
    }


    public static void ApplyPatch(string patchFile)
    {
        string filePath = AssetBundleManager.BaseLocalURL.Replace("file://", "") + patchFile;
        if(!File.Exists(filePath)){
            return;
        }
        GLog.Log(string.Format("ApplyPatch: {0}", patchFile));
        ZipInputStream s = new ZipInputStream(File.OpenRead(filePath));
        ZipEntry theEntry;
        while ((theEntry = s.GetNextEntry()) != null)
        {
            string directoryName = Path.GetDirectoryName(theEntry.Name);
            string fileName = Path.GetFileName(theEntry.Name);

            // create directory
            if (directoryName.Length > 0)
            {
                string dirPath = AssetBundleManager.BaseLocalURL.Replace("file://", "")+directoryName;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            if (fileName != String.Empty)
            {
                FileStream streamWriter = File.Create(AssetBundleManager.BaseLocalURL.Replace("file://", "")+theEntry.Name);
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = s.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        streamWriter.Write(data, 0, size);
                    }
                    else
                    {
                        streamWriter.Close();
                        break;
                    }
                }
            }
        }
    }

    public static bool SaveDataToLocal(string filePath, byte[] data)
    {
        string localFile = AssetBundleManager.BaseLocalURL.Replace("file://", "") + filePath;
        GLog.Log("localFile is:" + localFile);
        try
        {
            BinaryWriter writer = new BinaryWriter(File.Create(localFile));
            writer.Write(data);
            writer.Close();
        }
        catch (Exception e)
        {
            GLog.LogError(e.Message);
            return false;
        }
        return true;
    }

}
