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

    public void LoadAsset(string assetBundleName, string assetName, LoadCallback callback)
    {
        if (AssetBundleManager.DownloadedObjs.ContainsKey(assetBundleName))
        {
            callback(AssetBundleManager.DownloadedObjs[assetBundleName]);
            return;
        }
        StartCoroutine(LoadAssetAsync(assetBundleName, assetName, callback));
        AssetBundleManager.UnloadAssetBundle(assetBundleName);
    }

    public void LoadLevel(string assetBundleName, string levelName, bool isAdditive,ProgressCallback callback)
    {
        StartCoroutine(LoadLevelAsync(assetBundleName, levelName, isAdditive));
        AssetBundleManager.UnloadAssetBundle(assetBundleName);
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

    protected IEnumerator LoadAssetAsync(string assetBundleName, string assetName, LoadCallback callback)
    {
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(Object));
        if (request == null)
            yield break;
        yield return StartCoroutine(request);
        Object obj=request.GetAsset<Object>();
        if (obj != null){
            AssetBundleManager.DownloadedObjs.Add(assetBundleName,obj);
            callback(request.GetAsset<Object>());
        }else
            Debug.Log("loaded obj is null!");
  
    }

    protected IEnumerator LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
    {
        AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(assetBundleName, levelName, isAdditive);
        if (request == null)
            yield break;
        yield return StartCoroutine(request);        
    }
}
