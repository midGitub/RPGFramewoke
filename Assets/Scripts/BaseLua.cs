using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BaseLua : MonoBehaviour {
    private LuaScriptMgr umgr = null;

    public LuaScriptMgr UluaMrg
    {
        get
        {
            if (umgr == null)
            {
                umgr = ManagerStore.gameManager.uluaMgr;
            }
            return umgr;
        }
    }

    /// <summary>
    /// 执行Lua方法-无参数
    /// </summary>
    protected object[] CallMethod(string func)
    {
        if (UluaMrg == null) return null;
        string funcName = name + "." + func;
        funcName = funcName.Replace("(Clone)", "");
        return umgr.CallLuaFunction(funcName);
    }

    /// <summary>
    /// 执行Lua方法
    /// </summary>
    protected object[] CallMethod(string func, GameObject go)
    {
        if (UluaMrg == null) return null;
        string funcName = name + "." + func;
        funcName = funcName.Replace("(Clone)", "");
        return umgr.CallLuaFunction(funcName, go);
    }

    /// <summary>
    /// 执行Lua方法-Socket消息
    /// </summary>
    protected object[] CallMethod(string func, int key, ByteBuffer buffer)
    {
        if (UluaMrg == null) return null;
        string funcName = "Network." + func;
        funcName = funcName.Replace("(Clone)", "");
        return umgr.CallLuaFunction(funcName, key, buffer);
    }

    //-----------------------------------------------------------------
    protected void OnDestroy()
    {
        umgr = null;
        Util.ClearMemory();
        Debug.Log("~" + name + " was destroy!");
    }
}
