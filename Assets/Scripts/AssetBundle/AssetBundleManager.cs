using UnityEngine;
#if UNITY_EDITOR	
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class LoadedAssetBundle
{
	public AssetBundle m_AssetBundle;
	public int m_ReferencedCount;
	
	public LoadedAssetBundle(AssetBundle assetBundle)
	{
		m_AssetBundle = assetBundle;
		m_ReferencedCount = 1;
	}
}
public delegate void LoadCallback(string assetName,UnityEngine.Object obj,string extraInfo);
public delegate void ProgressCallback(float progress);

public class AssetBundleManager : SingletonBehaviour<AssetBundleManager>
{
    /// <summary>
    /// 本地资源加载路径
    /// </summary>
	static string m_BaseLocalURL = "";
	static string[] m_Variants =  {  };
	static AssetBundleManifest m_AssetBundleManifest = null;
	static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle> ();
	static Dictionary<string, WWW> m_DownloadingWWWs = new Dictionary<string, WWW> ();
	static Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string> ();
	static List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation> ();
	static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]> ();

    /// <summary>
    /// 缓存加载过的object
    /// </summary>
    static Dictionary<string, Object> m_DownloadedObjs = new Dictionary<string, Object>();

	public static string BaseLocalURL
	{
		get { return m_BaseLocalURL; }
		set { m_BaseLocalURL = value; }
	}

    public static Dictionary<string, Object> DownloadedObjs {
        get { return m_DownloadedObjs; }
    }

    public static Dictionary<string, WWW> DownloadingWWWS
    {
        get { return m_DownloadingWWWs; }
    }

	public static string[] Variants
	{
		get { return m_Variants; }
		set { m_Variants = value; }
	}

	public static AssetBundleManifest AssetBundleManifestObject
	{
		set {m_AssetBundleManifest = value; }
        get { return m_AssetBundleManifest; }
	}

	static public LoadedAssetBundle GetLoadedAssetBundle (string assetBundleName, out string error)
	{
        //如果加载过程中失败则返回null
		if (m_DownloadingErrors.TryGetValue(assetBundleName, out error) )
			return null;	
		LoadedAssetBundle bundle = null;
        //加载内容为空也返回null
		m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
		if (bundle == null)
			return null;
		string[] dependencies = null;
        //如果没有依赖则直接返回
		if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies) )
			return bundle;
		
		// Make sure all dependencies are loaded
		foreach(var dependency in dependencies)
		{
			if (m_DownloadingErrors.TryGetValue(assetBundleName, out error) )
				return bundle;

			// Wait all the dependent assetBundles being loaded.
			LoadedAssetBundle dependentBundle;
			m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
			if (dependentBundle == null)
				return null;
		}

		return bundle;
	}

    //加载配置文件
    static public AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName)
    {      
        LoadAssetBundle(manifestAssetBundleName, true);
        var operation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
        m_InProgressOperations.Add(operation);
        return operation;
    }

    static public AssetBundleLoadManifestOperation Initialize()
    {
        return Initialize(Utility.GetPlatformName());
    }

    public static void SetLocalAssetBundleDir()
    {
        BaseLocalURL = Utility.GetStreamingAssetsPath() + Utility.AssetBundlesOutputPath + Utility.GetPlatformName() + "/";
    }

	//加载该资源的所有依赖
	static protected void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest = false)
	{
		if (!isLoadingAssetBundleManifest)
			assetBundleName = RemapVariantName (assetBundleName);
		// Check if the assetBundle has already been processed.
		bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest);
		// Load dependencies.
		if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
			LoadDependencies(assetBundleName);
	}
	
	// Remaps the asset bundle name to the best fitting asset bundle variant.
	static protected string RemapVariantName(string assetBundleName)
	{
		string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();

		// If the asset bundle doesn't have variant, simply return.
		if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0 )
			return assetBundleName;

		string[] split = assetBundleName.Split('.');

		int bestFit = int.MaxValue;
		int bestFitIndex = -1;
		// Loop all the assetBundles with variant to find the best fit variant assetBundle.
		for (int i = 0; i < bundlesWithVariant.Length; i++)
		{
			string[] curSplit = bundlesWithVariant[i].Split('.');
			if (curSplit[0] != split[0])
				continue;
			
			int found = System.Array.IndexOf(m_Variants, curSplit[1]);
			if (found != -1 && found < bestFit)
			{
				bestFit = found;
				bestFitIndex = i;
			}
		}

		if (bestFitIndex != -1)
			return bundlesWithVariant[bestFitIndex];
		else
			return assetBundleName;
	}

	//从WWW加载资源
	static protected bool LoadAssetBundleInternal (string assetBundleName, bool isLoadingAssetBundleManifest)
	{
		// Already loaded.
		LoadedAssetBundle bundle = null;
		m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
		if (bundle != null)
		{
            //如果加载过了，直接引用次数+1
			bundle.m_ReferencedCount++;
			return true;
		}
        //如果正在加载，不作处理
		if (m_DownloadingWWWs.ContainsKey(assetBundleName) )
			return true;
		WWW download = null;
		string url = m_BaseLocalURL + assetBundleName;
		if (isLoadingAssetBundleManifest)
			download = new WWW(url);
		else
			download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(assetBundleName), 0); 
		m_DownloadingWWWs.Add(assetBundleName, download);
		return false;
	}

	//加载依赖
	static protected void LoadDependencies(string assetBundleName)
	{
		if (m_AssetBundleManifest == null)
		{
            GLog.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
			return;
		}
		//从配置文件获取所有依赖
		string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
		if (dependencies.Length == 0)
			return;			
		for (int i=0;i<dependencies.Length;i++)
			dependencies[i] = RemapVariantName (dependencies[i]);		
		//记录该资源所有引用
		m_Dependencies.Add(assetBundleName, dependencies);
		for (int i=0;i<dependencies.Length;i++)
			LoadAssetBundleInternal(dependencies[i], false);
	}

	//卸载该资源及其引用
	static public void UnloadAssetBundle(string assetBundleName)
	{
		UnloadAssetBundleInternal(assetBundleName);
		UnloadDependencies(assetBundleName);
	}

    static public void RemoveObject(string assetBundleName){
        if (m_DownloadedObjs.ContainsKey(assetBundleName))
        {
            m_DownloadedObjs.Remove(assetBundleName);
        }        
    }

    //卸载其依赖
	static protected void UnloadDependencies(string assetBundleName)
	{
		string[] dependencies = null;
		if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies) )
			return;
		// Loop dependencies.
		foreach(var dependency in dependencies)
		{
			UnloadAssetBundleInternal(dependency);
		}
		m_Dependencies.Remove(assetBundleName);
	}
    
    //卸载资源
	static protected void UnloadAssetBundleInternal(string assetBundleName)
	{
		string error;
		LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);
		if (bundle == null)
			return;

		if (--bundle.m_ReferencedCount == 0)
		{
			bundle.m_AssetBundle.Unload(false);
			m_LoadedAssetBundles.Remove(assetBundleName);
		}
	}

	void Update()
	{
		// Collect all the finished WWWs.
        List<string> keysToRemove = new List<string>();
        foreach (KeyValuePair<string, WWW> keyValue in m_DownloadingWWWs)
		{
			WWW download = keyValue.Value;
			// If downloading fails.
			if (download.error != null)
			{
				m_DownloadingErrors.Add(keyValue.Key, download.error);
				keysToRemove.Add(keyValue.Key);
				continue;
			}
			// If downloading succeeds.
			if(download.isDone)
			{
				m_LoadedAssetBundles.Add(keyValue.Key, new LoadedAssetBundle(download.assetBundle) );
				keysToRemove.Add(keyValue.Key);
			}
		}

		// Remove the finished WWWs.
		foreach( var key in keysToRemove)
		{
			WWW download = m_DownloadingWWWs[key];
			m_DownloadingWWWs.Remove(key);
			download.Dispose();
		}

		// Update all in progress operations
		for (int i=0;i<m_InProgressOperations.Count;)
		{
			if (!m_InProgressOperations[i].Update())
			{
				m_InProgressOperations.RemoveAt(i);
			}
			else
				i++;
		}
	}

	// Load asset from the given assetBundle.
	static public AssetBundleLoadAssetOperation LoadAssetAsync (string assetBundleName, string assetName, System.Type type)
	{
		AssetBundleLoadAssetOperation operation = null;
		LoadAssetBundle (assetBundleName);
		operation = new AssetBundleLoadAssetOperationFull (assetBundleName, assetName, type);
		m_InProgressOperations.Add (operation);
		return operation;
	}

	// Load level from the given assetBundle.
	static public AssetBundleLoadOperation LoadLevelAsync (string assetBundleName, string levelName, bool isAdditive)
	{
		AssetBundleLoadOperation operation = null;
		LoadAssetBundle (assetBundleName);
		operation = new AssetBundleLoadLevelOperation (assetBundleName, levelName, isAdditive);
		m_InProgressOperations.Add (operation);
		return operation;
	}
}