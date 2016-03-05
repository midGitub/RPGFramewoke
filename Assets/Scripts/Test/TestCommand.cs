using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestCommand : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Washer washer = new Washer();
        Command command = new WasherCommand(washer);
        CommanInvoker invoker = new CommanInvoker();
        invoker.AddCommand(command);
        invoker.Invoke();


        DefaultControls.Resources c = new DefaultControls.Resources();
        GameObject inputFiled=DefaultControls.CreateInputField(c);
        inputFiled.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
	}

}
