using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : SingletonObject<SceneManager>
{
    private Dictionary<string, XmlObj> xmlObjDic;

    private List<XmlObj> objs;

    private GameObject sceneParent;

    public void GenerateScene(SceneXmlObj sceneXlmObj) {
        objs=sceneXlmObj.Objs;
        if (objs == null || objs.Count == 0) {
            GLog.LogWarning("this is a empty scene!");
            return;
        }
        sceneParent = new GameObject("SceneRoot");
        xmlObjDic = new Dictionary<string, XmlObj>();
        foreach(XmlObj obj in objs){
            xmlObjDic.Add(obj.name,obj);           
        }
        AssetBundleService.getInstance().LoadAsset(objs[0].objPath, null, InitObj, objs[0].name);
    }

    private void InitObj(string assetName, UnityEngine.Object obj, string extraInfo)
    {
        XmlObj xmlObj=null;
        if (xmlObjDic.TryGetValue(extraInfo, out xmlObj)) {
            GameObject go = GameObject.Instantiate(obj, xmlObj.position, Quaternion.Euler(xmlObj.rotation)) as GameObject;
            go.name = xmlObj.name;
            go.transform.localScale = xmlObj.scale;
            go.transform.SetParent(sceneParent.transform);
        }
        objs.RemoveAt(0);
        SingletonObject<LoadingMediator>.getInstance().Progress = (xmlObjDic.Count - objs.Count) / (float)xmlObjDic.Count;
        if (objs.Count > 0) {          
            XmlObj xobj = objs[0];
            AssetBundleService.getInstance().LoadAsset(xobj.objPath, null, InitObj, xobj.name);
        } 
    }

    public void DestoryScene() {
        xmlObjDic.Clear();
        xmlObjDic = null;
        objs.Clear();
        objs = null; 
        GameObject.Destroy(sceneParent);
    }
}
