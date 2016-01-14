using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackView : MonoBehaviour {
    private GameObject prefab;

    public void RenderViewToModel(List<BasePackage> packs) {
        prefab = Resources.Load("PackItem") as GameObject;
        foreach(BasePackage pack in packs){
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.SetParent(transform);
            PackItem item=obj.GetComponent<PackItem>();
            item.SetModel(pack);
        }        
    }
}
