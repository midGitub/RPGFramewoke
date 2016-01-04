using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleService : SingletonBehaviour<AssetBundleService>
{
    private bool isLoadLevel = false;
    private string currentLevelName;
    private ProgressCallback currentCallback;

	// Use this for initialization
	void Start () {
        GameObject go = new GameObject("AssetBundleManager", typeof(AssetBundleManager));
        DontDestroyOnLoad(go);
        InitManifest();
	}

    public void InitManifest()
    {    
        StartCoroutine(LoadManifest());
    }

    public void LoadAsset(string assetBundleName, string assetName, LoadCallback callback,string extraInfo=null)
    {
        if (assetName == null)
            assetName = GainAssetName(assetBundleName);
        if (AssetBundleManager.DownloadedObjs.ContainsKey(assetBundleName))
        {
            callback(assetName, AssetBundleManager.DownloadedObjs[assetBundleName], extraInfo);
            AssetBundleManager.UnloadAssetBundle(assetBundleName);
            return;
        }
        StartCoroutine(LoadAssetAsync(assetBundleName, assetName, callback, extraInfo));      
    }

    private string GainAssetName(string assetBundleName)
    {
        string[] splits = assetBundleName.Split('/');
        return splits[splits.Length - 1].Split('.')[0];
    }

    public void LoadLevel(string assetBundleName, string levelName, bool isAdditive,ProgressCallback callback)
    {
        StartCoroutine(LoadLevelAsync(assetBundleName, levelName, isAdditive));      
        currentLevelName = assetBundleName;
        currentCallback = callback;
        isLoadLevel = true;
    }

    void Update() {
        if (isLoadLevel && currentLevelName!=null)
        {
            WWW www=AssetBundleManager.DownloadingWWWS[currentLevelName];
            if (!www.isDone)
                currentCallback(www.progress);
            else {
                AssetBundleManager.UnloadAssetBundle(currentLevelName);
                isLoadLevel = false;
                currentCallback = null;
                currentLevelName = null;
            }             
        }
    }

    protected IEnumerator LoadManifest()
    {
        var request = AssetBundleManager.Initialize();
        if (request != null)
            yield return StartCoroutine(request);
    }

    protected IEnumerator LoadAssetAsync(string assetBundleName, string assetName, LoadCallback callback, string extraInfo)
    {
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(Object));
        if (request == null)
            yield break;
        yield return StartCoroutine(request);
        Object obj=request.GetAsset<Object>();
        if (obj != null){
            AssetBundleManager.DownloadedObjs.Add(assetBundleName,obj);
            callback(assetName, request.GetAsset<Object>(), extraInfo);
            AssetBundleManager.UnloadAssetBundle(assetBundleName);
        }else
            GLog.Log("loaded obj is null!");
  
    }

    protected IEnumerator LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
    {
        AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(assetBundleName, levelName, isAdditive);
        if (request == null)
            yield break;
        yield return StartCoroutine(request);        
    }
}
