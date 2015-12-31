using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class XmlTools{

    /// <summary>
    /// 根据路径解析xml文件
    /// </summary>
    /// <param name="xmlPath"></param>
    /// <returns></returns>
    public static SceneXmlObj ParseXmlByPath(string xmlPath)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(xmlPath);
        return ParseXml(xmlDocument);
    }

    /// <summary>
    /// 根据xml文件内容解析
    /// </summary>
    /// <param name="xmlContent"></param>
    /// <returns></returns>
    public static SceneXmlObj ParseXmlByString(string xmlContent) {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlContent);
        return ParseXml(xmlDocument);   
    }

    private static SceneXmlObj ParseXml(XmlDocument xmlDocument)
    {
        SceneXmlObj sceneXmlObj = new SceneXmlObj();
        //获取场景名称
        string sceneName = xmlDocument.SelectNodes("//scene").Item(0).Attributes["sceneName"].Value;
        sceneXmlObj.SceneName = sceneName;
        // 使用 XPATH 获取所有 gameObject 节点
        XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//gameObject");
        List<XmlObj> xmlObjs = new List<XmlObj>();
        XmlObj xmlObj = null;
        foreach (XmlNode xmlNode in xmlNodeList)
        {
            xmlObj = new XmlObj();
            xmlObj.name = xmlNode.Attributes["objectName"].Value;
            xmlObj.objPath = xmlNode.Attributes["objectAsset"].Value;
            // 使用 XPATH 获取 位置、旋转、缩放数据
            XmlNode positionXmlNode = xmlNode.SelectSingleNode("descendant::position");
            XmlNode rotationXmlNode = xmlNode.SelectSingleNode("descendant::rotation");
            XmlNode scaleXmlNode = xmlNode.SelectSingleNode("descendant::scale");
            xmlObj.position = new Vector3(float.Parse(positionXmlNode.Attributes["x"].Value), float.Parse(positionXmlNode.Attributes["y"].Value), float.Parse(positionXmlNode.Attributes["z"].Value));
            xmlObj.rotation = new Vector3(float.Parse(rotationXmlNode.Attributes["x"].Value), float.Parse(rotationXmlNode.Attributes["y"].Value), float.Parse(rotationXmlNode.Attributes["z"].Value));
            xmlObj.scale = new Vector3(float.Parse(scaleXmlNode.Attributes["x"].Value), float.Parse(scaleXmlNode.Attributes["y"].Value), float.Parse(scaleXmlNode.Attributes["z"].Value));
            xmlObjs.Add(xmlObj);
        }
        sceneXmlObj.Objs = xmlObjs;
        xmlDocument = null;
        return sceneXmlObj;
    }

    public static void test() {
        AssetBundleService.getInstance().LoadAsset("staticdatas/testscene2xml.unity3d",null, testPrefabs);
    }

    private static void testPrefabs(string assetName, UnityEngine.Object obj,string extraInfo)
    {
        FileStreamHolder file = GameObject.Instantiate(obj as FileStreamHolder);
        string content=file.Content;
        Debug.Log("content-------------"+content);
        SceneXmlObj  xmlObj= ParseXmlByString(file.Content);
        SceneBuilder.getInstance().GenerateScene(xmlObj);

        //Debug.Log("xmlObj-------------" + xmlObj.SceneName);
    }
}
