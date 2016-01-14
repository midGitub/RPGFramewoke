using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour {
    public PackController controller;

	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.A)){
            controller.ShowPack();
        }
	}
}
