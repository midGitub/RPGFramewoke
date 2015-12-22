using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool instance;

    private Dictionary<string, List<GameObject>> allObj;
    //最多能缓存物体的种类数
    public int perTypeMaxSize = 5;
    //每种缓存的物体的初始大小
    public int perTypePreSize = 5;

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
    private void initNewTypeObj(string name)
    {
        List<GameObject> preRes = new List<GameObject>();
        GameObject template = Resources.Load(name) as GameObject;
        for (int i = 0; i < perTypePreSize; i++)
        {
            preRes.Add(initObj(template));
        }
        allObj.Add(name,preRes);
    }

    //实例化一个模板对象
    private GameObject initObj(GameObject template,bool dontDestroyOnLoad=false)
    {
        GameObject entity = Instantiate(template, Vector3.zero, Quaternion.identity) as GameObject;
        entity.transform.parent = transform;
        entity.SetActive(false);
        if(dontDestroyOnLoad)
            DontDestroyOnLoad(entity);
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
        //如果没有该种类的则继续实例化新的种类
        if (allObj.Count < perTypeMaxSize) {
            initNewTypeObj(name);
            return GetObjByName(name);
        }
        return null;
    }

    public void DestroyObj(GameObject obj) {
        obj.SetActive(false);
    }

    public List<string> AllPreRes {
        get { return allPreRes; }
        set { allPreRes = value; }
    }
}
