using UnityEngine;
using System.Collections;

public class GLog{
    static bool isEditor=false;

    public static void Log(object msg,bool isImportant=false)
    {
#if UNITY_EDITOR || UNITY_WEBPLAYER
        isEditor = true;
#endif
        if(isEditor || isImportant)
            Debug.Log(msg);
    }

    public static void LogWarning(object msg, bool isImportant = false)
    {
#if UNITY_EDITOR || UNITY_WEBPLAYER
        isEditor = true;
#endif
        if(isEditor || isImportant)
            Debug.LogWarning(msg); 
    }

    public static void LogError(object msg, bool isImportant = false)
    {
#if UNITY_EDITOR || UNITY_WEBPLAYER
        isEditor = true;
#endif
        if(isEditor || isImportant)
            Debug.LogError(msg); 
    }

    //unity里vector3的debug日志是精确到0.1的，因此必须用该函数来打印日志，这是官方推荐的
    public static void DebugVector3(Vector3 v)
    {
        Log("(" + v.x + "," + v.y + "," + v.z + ")");
    }
}
