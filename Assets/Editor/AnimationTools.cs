using UnityEngine;
using System.Collections;
using UnityEditor;

public class AnimationTools : Editor {

    [MenuItem("Tools/Animation/Create Model Prefab")]
    public static void CreateModelPrefab()
    {
        string path = AssetDatabase.GetAssetOrScenePath(Selection.activeObject);
        if (path.Contains("@"))
        {
            path = path.Substring(0, path.LastIndexOf("@"));
        }
        else
        {
            path = path.Substring(0, path.LastIndexOf("."));
        }

        Object obj = AssetDatabase.LoadAssetAtPath(path + ".fbx", typeof(Object));
        if (obj == null) return;

        GameObject go = PrefabUtility.InstantiatePrefab(obj) as GameObject;
        if (go == null) return;

        string directory = path.Substring(0, path.LastIndexOf("/"));
        directory = directory.Replace("Assets", "") + "/" + go.name;

        GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets" + directory + ".prefab", typeof(Object)) as GameObject;
        if (prefab == null)
        {
            prefab = PrefabUtility.CreatePrefab("Assets" + directory + ".prefab", go);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(prefab));
        }
        Object.DestroyImmediate(go);
    }
}
