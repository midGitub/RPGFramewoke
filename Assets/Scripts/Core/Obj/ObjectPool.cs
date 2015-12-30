using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// author:wangying
/// </summary>
public class ObjectPool : MonoBehaviour {
    public static ObjectPool instance;

    private Dictionary<string, List<GameObject>> allObj;
    //最多能缓存物体的种类数(暂时)
    private int perTypeMaxSize = 5;

    private List<string> allPreRes;

    void Awake() {
        instance = this;
        allObj = new Dictionary<string, List<GameObject>>();
        allPreRes = new List<string>();
    }

	// Use this for initialization
	void Start () {
        initPool();
	}

    private void initPool() {
        foreach (string res in allPreRes)
        {
            initNewTypeObj(res);
        }
    }

    //初始化一个新类型的object
    private void initNewTypeObj(string name,int initCount=3)
    {
        List<GameObject> preRes = new List<GameObject>();
        GameObject template = Resources.Load(name) as GameObject;
        for (int i = 0; i < initCount; i++)
        {
            preRes.Add(initObj(template));
        }
        if (allObj.ContainsKey(name))
            allObj[name] = preRes;
        else
            allObj.Add(name, preRes);
    }

    //实例化一个模板对象
    private GameObject initObj(GameObject template)
    {
        GameObject entity = Instantiate(template, Vector3.zero, Quaternion.identity) as GameObject;
        entity.transform.parent = transform;
        entity.SetActive(false);
        return entity;
    }

    //初始化一个已有种类的对象
    private void initExtraObj(string name) {
        GameObject template = Resources.Load(name) as GameObject;
        allObj[name].Add(initObj(template));
    }

    public GameObject GetObjByName(string name) {
        if(allObj.ContainsKey(name)){
            List<GameObject> objs=allObj[name];
            //直接去找没有激活的对象，然后返回
            foreach (GameObject obj in objs) {
                if (!obj.activeSelf) {
                    obj.SetActive(true);
                    return obj;
                }
            }
            //如果没有找到继续实例化
            initExtraObj(name);
            return GetObjByName(name);
        }
        //如果超出了最大种类上限
        if (allObj.Count >= perTypeMaxSize)
            deleteOrIncreaseType();
        initNewTypeObj(name);
        return GetObjByName(name);
    }

    //删除现有种类或者强制增加一个种类
    private void deleteOrIncreaseType() {
        //保存可以删除的种类
        List<string> types = new List<string>();
        foreach (KeyValuePair<string, List<GameObject>> kv in allObj)
        {
            bool canAdd = true;
            foreach (GameObject obj in kv.Value)
            {
                if (obj.activeSelf)
                {
                    canAdd = false;
                    break;
                }
            }
            if (canAdd)
                types.Add(kv.Key);
        }
        //如果存在可以删除的种类，则从中选择个数最少的进行删除
        if (types.Count != 0)
        {
            int minCount = 0;
            string deleteName = null;
            foreach (string tName in types)
            {
                int tCount = allObj[tName].Count;
                if (tCount > minCount)
                {
                    minCount = tCount;
                    deleteName = tName;
                }
            }
            deleteObjType(deleteName);
        }
        //如果没有可删除的，强制增长种类数
        else
            perTypeMaxSize++;
    }

    //删除某种种类的obj
    private bool deleteObjType(string name,bool forceDelete=false) {
        if (!allObj.ContainsKey(name))
            return false;
        List<GameObject> objs=allObj[name];
        bool canRemove=true;
        //如果不为强制删除模式，则去判断一下是否有激活obj
        if (!forceDelete) {
            foreach (GameObject go in objs)
            {
                //如果有激活状态的obj,则不允许删除
                if (go.activeSelf)
                {
                    canRemove = false;
                    break;
                }
            }
        }    
        if (canRemove)
        {
            foreach (GameObject go in objs)
            {
                GameObject.Destroy(go);
            }
            objs.Clear();
            objs = null;
            allObj.Remove(name);
            return true;
        }else
            return false;
    }

    //清空缓存池
    public void clearPool() {
        string[] keys = new string[allObj.Count];
        allObj.Keys.CopyTo(keys,0);
        foreach (string key in keys)
        {
            deleteObjType(key, true);
        }
        allObj.Clear();
    }

    public void DestroyObj(GameObject obj) {
        obj.SetActive(false);
    }

    public List<string> AllPreRes {
        get { return allPreRes; }
        set { allPreRes = value; }
    }

    public int PerTypeMaxSize {
        get { return perTypeMaxSize; }
        set { perTypeMaxSize = value; }
    }
}
