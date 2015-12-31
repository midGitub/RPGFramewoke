using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;


public class ExportSceneToXml : Editor 
{
    //默认存放场景xml文件的文件夹
    private static string defaultDir = "/StaticDatas/";

	[MenuItem("Tools/Export Scene To XML From Selection")]
	static void ExportXML()
	{
		Object[] selectedAssetList = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);			
		//遍历所有的游戏对象
		foreach (Object selectObject in selectedAssetList) 
		{
			// 场景名称
			string sceneName = selectObject.name;
			// 场景路径
			string scenePath = AssetDatabase.GetAssetPath(selectObject);
            Debug.Log("scenePath--------------" + scenePath);
            string[] paths=sceneName.Split('/');
            string fileName=paths[paths.Length - 1].Split('.')[0];
            string path = Application.dataPath + defaultDir + fileName + ".xml";
			// 如果存在场景文件，删除
			if(File.Exists(path)) File.Delete(path);
			// 打开这个关卡
			EditorApplication.OpenScene(scenePath);
			XmlDocument xmlDocument = new XmlDocument();
			// 创建XML属性
			XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
			xmlDocument.AppendChild(xmlDeclaration);
			// 创建XML根标志
			XmlElement rootXmlElement = xmlDocument.CreateElement("root");
			// 创建场景标志
			XmlElement sceneXmlElement = xmlDocument.CreateElement("scene");
			sceneXmlElement.SetAttribute("sceneName", sceneName);
				
			foreach (GameObject sceneObject in Object.FindObjectsOfType(typeof(GameObject)))
			{
				// 如果对象是激活状态
				if (sceneObject.transform.parent == null && sceneObject.activeSelf)
				{
					// 判断是否是预设
					if(PrefabUtility.GetPrefabType(sceneObject) == PrefabType.PrefabInstance)
					{
						// 获取引用预设对象
                        Object prefabObject = PrefabUtility.GetPrefabParent(sceneObject);
                        string prefabsPath=AssetDatabase.GetAssetPath(prefabObject);
                        prefabsPath = prefabsPath.Replace("Assets/", "").Replace(".prefab", ".unity3d").ToLower();
                        Debug.Log("prefabsPath---------" + prefabsPath);
						if(prefabObject != null)
						{
							XmlElement gameObjectXmlElement = xmlDocument.CreateElement("gameObject");
							gameObjectXmlElement.SetAttribute("objectName", sceneObject.name);
                            gameObjectXmlElement.SetAttribute("objectAsset", prefabsPath);
								
							XmlElement transformXmlElement = xmlDocument.CreateElement("transform");
								
							// 位置信息
							XmlElement positionXmlElement = xmlDocument.CreateElement("position");
							positionXmlElement.SetAttribute("x", sceneObject.transform.position.x.ToString());
							positionXmlElement.SetAttribute("y", sceneObject.transform.position.y.ToString());
							positionXmlElement.SetAttribute("z", sceneObject.transform.position.z.ToString());
								
							// 旋转信息
							XmlElement rotationXmlElement = xmlDocument.CreateElement("rotation");
							rotationXmlElement.SetAttribute("x", sceneObject.transform.rotation.eulerAngles.x.ToString());
							rotationXmlElement.SetAttribute("y", sceneObject.transform.rotation.eulerAngles.y.ToString());
							rotationXmlElement.SetAttribute("z", sceneObject.transform.rotation.eulerAngles.z.ToString());
								
							// 缩放信息
							XmlElement scaleXmlElement = xmlDocument.CreateElement("scale");
							scaleXmlElement.SetAttribute("x", sceneObject.transform.localScale.x.ToString());
							scaleXmlElement.SetAttribute("y", sceneObject.transform.localScale.y.ToString());
							scaleXmlElement.SetAttribute("z", sceneObject.transform.localScale.z.ToString());
								
							transformXmlElement.AppendChild(positionXmlElement);
							transformXmlElement.AppendChild(rotationXmlElement);
							transformXmlElement.AppendChild(scaleXmlElement);    
								
							gameObjectXmlElement.AppendChild(transformXmlElement);
							sceneXmlElement.AppendChild(gameObjectXmlElement);
						}
					}
				}
			}
			rootXmlElement.AppendChild(sceneXmlElement);
			xmlDocument.AppendChild(rootXmlElement);
			// 保存场景数据
			xmlDocument.Save(path);
			// 刷新Project视图
			AssetDatabase.Refresh();
		}
	}
}
