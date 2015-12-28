using UnityEngine;
using System.Collections;

public class ObjectManager : SingletonObject<ObjectManager> {

    public void Init() {
        GameObject objPoll = new GameObject("ObjectPool");
        objPoll.AddComponent<ObjectPool>();
        GameObject.DontDestroyOnLoad(objPoll);
    }
}
