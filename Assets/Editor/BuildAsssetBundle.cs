
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text;

#if UNITY_EDITOR
public class BuildAsssetBundle : Editor
{
    //存放所有资源文件的最外层文件夹名字
	const string AssetBundlesOutputPath = "AssetBundles";
    //存放临时文件的文件夹
    const string dumpPath = "Dumps";

    /// <summary>
    /// 删除所有Dumps里面的文件
    /// </summary>
    public static void ClearAllDumps() {
        string[] fileNames=Directory.GetFiles(Path.Combine(Application.dataPath,dumpPath));
        foreach(string fileName in fileNames){
            File.Delete(fileName);
        }
    }


    //----------------------------------------------打包各平台资源----------------------------------------------

    static void Exec(string subdirectory, BuildTarget target)
    {
        string outputPath = Path.Combine(AssetBundlesOutputPath, subdirectory);
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, target);
        //ClearAllDumps();
        AssetDatabase.Refresh();
    }
	[MenuItem("[Build AsssetBundle]/Build For  [Windows]")]
    static void ExecuteForWindows()
    {
        Exec("Windows", BuildTarget.WebPlayer);
 
    }

	[MenuItem("[Build AsssetBundle]/Build For  [Android]")]
    static void ExecuteForAndroid()
    {
        Exec("Android", BuildTarget.Android);

    }

	[MenuItem("[Build AsssetBundle]/Build For  [IOS]")]
    static void ExecuteForIOS()
    {
        Exec("IOS", BuildTarget.iOS);

    }
    //----------------------------------------------打包各平台资源----------------------------------------------


    //----------------------------------------------给资源添加AssetLabels---------------------------------------------

    static void NameAssetBundle(string suffix, string newSuffix = ".unity3d")
    {
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (!path.Contains(".") || !path.Contains(suffix))
            {
                continue;
            }
            Debug.Log("path:" + path);
            string assetName = path.Replace("Assets/", "").Replace(suffix, newSuffix);
            Debug.Log("assetName:" + assetName);
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            assetImporter.assetBundleName = assetName;
            //保存
            assetImporter.SaveAndReimport();
        }
    }

    [MenuItem("[Build AsssetBundle]/[NameAssetBundle]/All")]
    static void NameAll()
    {
        NamePrefabs();
        NameMaterials();
        NameTextures();
        NameAudio();
        NameStaticData();
        NameScene();
    }

	[MenuItem("[Build AsssetBundle]/[NameAssetBundle]/Prefab")]
	static void NamePrefabs(){
		NameAssetBundle (".prefab");
	}

	[MenuItem("[Build AsssetBundle]/[NameAssetBundle]/Material")]
	static void NameMaterials(){
		NameAssetBundle (".mat");
	}

	[MenuItem("[Build AsssetBundle]/[NameAssetBundle]/Texture")]
	static void NameTextures(){
		NameAssetBundle (".png");
	}

    [MenuItem("[Build AsssetBundle]/[NameAssetBundle]/Audio")]
    static void NameAudio()
    {
        NameAssetBundle(".ogg");
    }

    [MenuItem("[Build AsssetBundle]/[NameAssetBundle]/Scene")]
    static void NameScene()
    {
        NameAssetBundle(".unity");
    }

    [MenuItem("[Build AsssetBundle]/[NameAssetBundle]/StaticData")]
    static void NameStaticData() {
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string filePath=AssetDatabase.GetAssetPath(obj);
            if(!filePath.Contains(".txt")){
                continue;
            }
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.ASCII);
            string line;
            StringBuilder sb = new StringBuilder();
            while(true){
                line=reader.ReadLine();
                if (line != null && line != "\r\n")
                {
                    sb.Append(line);
                }
                else {
                    break;
                }
            }
            string content = sb.ToString();
            string fileName=Path.GetFileNameWithoutExtension(filePath);
            FileStreamHolder holder = ScriptableObject.CreateInstance<FileStreamHolder>();
            holder.Content = content;
            holder.FileName = fileName;
            //临时文件的路径
            string tempPath = Path.Combine(Path.Combine("Assets",dumpPath),fileName)+ ".asset";
            //生成临时文件
            AssetDatabase.CreateAsset(holder, tempPath);
            //然后改名(等打包完毕再删除)
            AssetImporter assetImporter = AssetImporter.GetAtPath(tempPath);
            assetImporter.assetBundleName =Path.Combine("StaticDatas",fileName) + ".unity3d";
            assetImporter.SaveAndReimport();
        }
    }
    //----------------------------------------------给资源添加AssetLabels---------------------------------------------


    //----------------------------------------------清除资源上的AssetLabels---------------------------------------------


    static void ClearAssetBundle(string suffix, string newSuffix = ".unity3d")
    {
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (!path.Contains(".") || !path.Contains(suffix))
            {
                continue;
            }
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            assetImporter.assetBundleName = null;
            //保存
            assetImporter.SaveAndReimport();
        }
    }

    [MenuItem("[Build AsssetBundle]/[ClearAssetBundle]/All")]
    static void ClearAll()
    {
        ClearAssetBundle(".prefab");
        ClearAssetBundle(".mat");
        ClearAssetBundle(".png");
        ClearAssetBundle(".ogg");
        ClearAssetBundle(".unity");
        ClearAssetBundle(".controller");
        ClearAssetBundle(".asset");
        ClearAssetBundle(".jpg");
        ClearAllDumps();
    }

    [MenuItem("[Build AsssetBundle]/[ClearAssetBundle]/Prefab")]
    static void ClearPrefabs()
    {
        ClearAssetBundle(".prefab");
    }

    [MenuItem("[Build AsssetBundle]/[ClearAssetBundle]/Material")]
    static void ClearMaterial()
    {
        ClearAssetBundle(".mat");
    }

    [MenuItem("[Build AsssetBundle]/[ClearAssetBundle]/Texture")]
    static void ClearTexture()
    {
        ClearAssetBundle(".png");
    }

    [MenuItem("[Build AsssetBundle]/[ClearAssetBundle]/Audio")]
    static void ClearAudio()
    {
        ClearAssetBundle(".ogg");
    }

    [MenuItem("[Build AsssetBundle]/[ClearAssetBundle]/Scene")]
    static void ClearScene()
    {
        ClearAssetBundle(".unity");
    }
    //----------------------------------------------清除资源上的AssetLabels---------------------------------------------

    /*
    [MenuItem("[Build AsssetBundle]/[NameAssetBundle]/LightMap")]
    static void StoreAndRenameLightMap()
    {
        LightMapAsset lightmapAsset = ScriptableObject.CreateInstance<LightMapAsset>();
        int iCount = LightmapSettings.lightmaps.Length;
        if(iCount==0){
            return;
        }
        lightmapAsset.lightmapFar = new Texture2D[iCount];
        lightmapAsset.lightmapNear = new Texture2D[iCount];
        
        for (int i = 0; i < iCount; ++i)
        {
            // 这里直接把lightmap纹理存起来
            lightmapAsset.lightmapFar[i] = LightmapSettings.lightmaps[i].lightmapFar;
            lightmapAsset.lightmapNear[i] = LightmapSettings.lightmaps[i].lightmapNear;
        }
        string tempAssetPath = Path.Combine(Path.Combine("Assets", dumpPath), "lightmap.asset");
        //创建lightmap
        AssetDatabase.CreateAsset(lightmapAsset, tempAssetPath);
        AssetImporter assetImporter = AssetImporter.GetAtPath(tempAssetPath);
        assetImporter.assetBundleName = "lightmap.unity3d";
        assetImporter.SaveAndReimport();
    }*/



    //----------------------------------------------测试未开放----------------------------------------------

	//测试获取资源名称
	[MenuItem ("Tools/Show All AssetBundle names")]
	static void GetNames ()
	{
		string[] names = AssetDatabase.GetAllAssetBundleNames();
		foreach (string name in names)
			Debug.Log ("AssetBundle: " + name);
	}

}

#endif