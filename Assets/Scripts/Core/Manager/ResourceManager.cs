using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : SingletonObject<ResourceManager> {
    private Dictionary<string, eStaticDataType> m_StaticDict = new Dictionary<string, eStaticDataType>();

    public void init() {
        m_StaticDict.Add("skill",eStaticDataType.STATICDATA_SKILL);
    }

    public void StartDownLoad() {
        AssetBundleService  abService= AssetBundleService.getInstance();
        foreach (KeyValuePair<string, eStaticDataType> kvp in m_StaticDict)
        {
            abService.LoadAsset("staticdatas/" + kvp.Key + ".unity3d", kvp.Key, LoadStaticDataCompleted);
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

    public void ReleaseResources() {
        m_StaticDict.Clear();
    }
}
