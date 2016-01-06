using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class Tools : Editor {

    /// <summary>
    /// 递归删除选中物体的碰撞
    /// </summary>
    [MenuItem("Tools/Remove Collider")]
    static void ExeDeleteCollider() {
        GameObject[] objs=Selection.gameObjects;
        foreach (GameObject o in objs)
        {
            Collider[] colliders = o.GetComponentsInChildren<Collider>();
            for (int i = 0; i < colliders.Length; ++i)
            {
                UnityEngine.Object.DestroyImmediate(colliders[i]);
            }
        }
    }

    private static string dirName = "StreamingAssets/";
    private static string fileName = "files.txt";

    /// <summary>
    /// 生成该目录下所有文件的一个文件列表(暂时仅针对StreamingAssets目录)
    /// </summary>
    [MenuItem("Tools/GenerateFileList")]
    static void GenerateFileList() {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(filePath))
            File.Delete(filePath);
        FileStream fs1 = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs1);
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(obj);          
            //如果是.meta文件，或者不是StreamingAssets路径下的，或者是文件夹
            if (path.Contains(".meta") || !path.Contains(dirName) || !path.Contains("."))
            {
                continue;
            }
            string result=Util.CutString(path, dirName);
            sw.WriteLine(result);
            Debug.Log("result:"+result);        
        }
        sw.Close();
        fs1.Close();
        AssetDatabase.Refresh();
    }
}
