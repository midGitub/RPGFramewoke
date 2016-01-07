using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Util {

    /// <summary>
    /// 取得Lua路径
    /// </summary>
    public static string LuaPath(string name) {       
        //如果不是ulua自带的lua脚本
        if (name.Contains(Utility.AssetBundlesOutputPath)) {
            return name;            
        }
        string path = null;
        if (Application.isEditor)
        {
            path = Application.dataPath;
        }
        else if (Application.isMobilePlatform || Application.isConsolePlatform)
        {
            path = Application.persistentDataPath + "/AssetBundles";
        }
        else
            path = "file://" + Application.streamingAssetsPath + "/AssetBundles";
        string lowerName = name.ToLower();
        if (lowerName.EndsWith(".lua")) {
            int index = name.LastIndexOf('.');
            name = name.Substring(0, index);
        }
        name = name.Replace('.', '/');
        return path + "/ulua/Lua/" + name + ".lua";
    }

    public static void Log(string str) {
        Debug.Log(str);
    }

    public static void LogWarning(string str) {
        Debug.LogWarning(str);
    }

    public static void LogError(string str) {
        Debug.LogError(str);
    }

    /// <summary>
    /// 清理内存
    /// </summary>
    public static void ClearMemory() {
        GC.Collect();
        Resources.UnloadUnusedAssets();
        LuaScriptMgr mgr = LuaScriptMgr.Instance;
        if (mgr != null && mgr.lua != null) mgr.LuaGC();
    }

    /// <summary>
    /// 防止初学者不按步骤来操作
    /// </summary>
    /// <returns></returns>
    static int CheckRuntimeFile() {
        if (!Application.isEditor) return 0;
        string sourceDir = AppConst.uLuaPath + "/Source/LuaWrap/";
        if (!Directory.Exists(sourceDir)) {
            return -2;
        } else {
            string[] files = Directory.GetFiles(sourceDir);
            if (files.Length == 0) return -2;
        }
        return 0;
    }

    /// <summary>
    /// 检查运行环境
    /// </summary>
    public static bool CheckEnvironment() {
#if UNITY_EDITOR
        int resultId = Util.CheckRuntimeFile();
        if (resultId == -1) {
            Debug.LogError("没有找到框架所需要的资源，单击Game菜单下Build xxx Resource生成！！");
            EditorApplication.isPlaying = false;
            return false;
        } else if (resultId == -2) {
            Debug.LogError("没有找到Wrap脚本缓存，单击Lua菜单下Gen Lua Wrap Files生成脚本！！");
            EditorApplication.isPlaying = false;
            return false;
        }
#endif
        return true;
    }
    /// <summary>
    /// 是不是苹果平台
    /// </summary>
    /// <returns></returns>
    public static bool isApplePlatform {
        get {
            return Application.platform == RuntimePlatform.IPhonePlayer ||
                   Application.platform == RuntimePlatform.OSXEditor ||
                   Application.platform == RuntimePlatform.OSXPlayer;
        }
    }


    /// <summary>
    /// 目标目录
    /// </summary>
    public static string ToPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath;
            }
            return Application.dataPath.Replace("/Assets","") + "/Sample";
        }
    }

    /// <summary>
    /// 源目录
    /// </summary>
    public static string FromPath {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.streamingAssetsPath;
            }
            return Application.dataPath + "/StreamingAssets";
        }
    }
    
    /// <summary>
    /// 获取指定字符串后面的所有内容
    /// </summary>
    public static string CutString(string source,string removeStr) {
        if (!source.Contains(removeStr))
            return source;
        int index=source.IndexOf(removeStr);
        return source.Substring(index + removeStr.Length);
    }
}