using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TKObjManager{
    private static Dictionary<string, GameObject> mObjects = new Dictionary<string, GameObject>();


    public static GameObject FindObject(string name) {
        if (mObjects.ContainsKey(name))
            return mObjects[name];
        else
            return GameObject.Find(name);
    }
}
