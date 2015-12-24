using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LuaManager : BaseLua
{
    private List<string> luas = new List<string>();
    public LuaScriptMgr uluaMgr;

    private void Init() {
        foreach(string path in luas){
            uluaMgr.DoFile(path);
        }
    }

	// Use this for initialization
	void Awake () {
        uluaMgr = new LuaScriptMgr();
        uluaMgr.Start();
        string baseUrl = AssetBundleManager.BaseLocalURL.Replace("file://", "");
        luas.Add(baseUrl + "testlua.lua");
	}

    void Start() {
        Init();
    }
}
