using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TestUnityEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StringEvent se = new StringEvent();
        se.AddListener(Test);
        se.AddListener(Test1);
        se.Invoke("sssssssss",11);
        StartCoroutine("Test3");

	}
	
    IEnumerator Test2() {
        Debug.Log("---------------test21");
        yield return null;
        Debug.Log("----------test22");
    }

    private void Test3() {
        Debug.Log("----------test3");
    }

    private void Test(string s,int i) {
        Debug.Log("test event:"+s+"---"+i);
    }

    private void Test1(string s1,int i1) {
        Debug.Log("test1 event:"+s1+"----"+i1);
    }
}

class StringEvent : UnityEvent<string,int> { 
    
}