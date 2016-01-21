using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour {

    public void SayHi(int i) {
        GLog.Log("hello"+i);
    }
}
