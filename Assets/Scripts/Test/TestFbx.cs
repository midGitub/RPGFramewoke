using UnityEngine;
using System.Collections;

public class TestFbx : MonoBehaviour {
    public GameObject parent;

    public void SayHello(int i) {
        if(parent!=null)
            parent.SendMessage("SayHi",i);
    }
}
