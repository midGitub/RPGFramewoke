using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneBuilder : SingletonObject<SceneBuilder>{
    private Dictionary<string, XmlObj> xmlObjDic;

    private List<XmlObj> objs;

    public void GenerateScene(SceneXmlObj sceneXlmObj) {
        objs=sceneXlmObj.Objs;
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
        }
        //xmlObjDic.Remove(extraInfo);
        //if (xmlObjDic.Count!=0)
        //    AssetBundleService.getInstance().LoadAsset(objs[0].objPath, null, InitObj, objs[0].name);
        
    }
}
