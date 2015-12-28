using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LuaManager : SingletonObject<LuaManager>
{
    private List<string> luas = new List<string>();
    public LuaScriptMgr uluaMgr;

    public void Init() {
        uluaMgr = new LuaScriptMgr();
        uluaMgr.Start();
        string baseUrl = AssetBundleManager.BaseLocalURL.Replace("file://", "");
        luas.Add(baseUrl + "testlua.lua");       
    }

    public void LoadFile() {
        foreach (string path in luas)
        {
            uluaMgr.DoFile(path);
        }
    }

    /// <summary>
    /// 执行Lua方法-文件名、方法名
    /// </summary>
    public object[] CallMethod(string fileName, string func)
    {
        if (uluaMgr == null) return null;
        string funcName = fileName + "." + func;
        return uluaMgr.CallLuaFunction(funcName);
    }

    //-----------------------------------------------------------------
    public void OnDestroy()
    {
        uluaMgr = null;
        Util.ClearMemory();
        GLog.Log("~ BaseLua was destroy!");
    }
}
