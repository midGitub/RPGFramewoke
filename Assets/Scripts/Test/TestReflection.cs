using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Runtime.Remoting;

public class TestReflection : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Type aa=typeof(AA);
        System.Reflection.BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
        MethodInfo method=aa.GetMethod("test", flags);
        object[] s=new object[]{1};
        method.Invoke(Activator.CreateInstance(aa),s);
	}
}

class AA {
    public void test(int i) {
        Debug.Log("反射一下------------"+i);
    }
}