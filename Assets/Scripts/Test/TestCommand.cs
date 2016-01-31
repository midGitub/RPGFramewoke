using UnityEngine;
using System.Collections;

public class TestCommand : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Washer washer = new Washer();
        Command command = new WasherCommand(washer);
        CommanInvoker invoker = new CommanInvoker();
        invoker.AddCommand(command);
        invoker.Invoke();
	}

}
