using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : SingletonObject<ResourceManager> {
    private Dictionary<string, eStaticDataType> m_StaticDict = new Dictionary<string, eStaticDataType>();

    private Dictionary<string, BaseMediator> m_mediatorDic = new Dictionary<string, BaseMediator>();

    public void Init() {
        m_StaticDict.Add("skill",eStaticDataType.STATICDATA_SKILL);

        m_mediatorDic.Add("panel_test", SingletonObject<TestMediator>.getInstance());
        m_mediatorDic.Add("panel_login", SingletonObject<LoginMediator>.getInstance());
        m_mediatorDic.Add("panel_loading", SingletonObject<LoadingMediator>.getInstance());
    }

    public void StartDownLoad() {
        AssetBundleService  abService= AssetBundleService.getInstance();
        foreach (KeyValuePair<string, eStaticDataType> kvp in m_StaticDict)
        {
            abService.LoadAsset("staticdatas/" + kvp.Key + ".unity3d", kvp.Key, LoadStaticDataCompleted);
        }

        UIManager uiManager= UIManager.getInstance();
        foreach (KeyValuePair<string, BaseMediator> kv in m_mediatorDic)
        {
            kv.Value.Path ="prefabs/ui/"+ kv.Key+".unity3d";
            kv.Value.PanelName = kv.Key;
            kv.Value.LoadPanel();
            uiManager.AddMediator(kv.Value);
        }
    }

    private void LoadStaticDataCompleted(string assetName,UnityEngine.Object asset)
    {
        if (!m_StaticDict.ContainsKey(assetName))
            return;
        FileStreamHolder t = asset as FileStreamHolder;
        eStaticDataType type = m_StaticDict[assetName];
        switch (type) { 
            case eStaticDataType.STATICDATA_SKILL:
                CStaticDownload<CSkillLoader>.getInstance().LoadCompleted(t.Content);
                break;
        }
    }

    public void OnDestroy()
    {
        m_StaticDict.Clear();
    }
}
