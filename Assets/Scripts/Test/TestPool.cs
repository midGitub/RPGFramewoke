using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestPool : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject go = new GameObject("ObjectPool");
        go.AddComponent<ObjectPool>();
        DontDestroyOnLoad(go);
        foreach (object s in Test())
            GLog.Log(s);
        GLog.Log("yield end");
	}
	
        IEnumerable Test()
        {
          yield return 5;
          yield return 1000;
          yield break;
        }

    void OnGUI() { 
        if(GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 150, 300, 50), "初始化物体")){
            GameObject capsule=ObjectPool.instance.GetObjByName("Prefabs/Capsule");
            GameObject cube = ObjectPool.instance.GetObjByName("Prefabs/Cube");
            GameObject sphere = ObjectPool.instance.GetObjByName("Prefabs/Sphere");
            int range1=Random.Range(-4, 0);
            capsule.transform.position = new Vector3(range1, range1, range1);
            int range2 = Random.Range(0, 4);
            cube.transform.position = new Vector3(range2, range2, range2);
            int range3 = Random.Range(4, 8);
            sphere.transform.position = new Vector3(range3, range3, range3);
        }
    }
}
