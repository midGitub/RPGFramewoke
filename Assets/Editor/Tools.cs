using UnityEngine;
using UnityEditor;
using System.Collections;

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
}
