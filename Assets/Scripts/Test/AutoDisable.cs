using UnityEngine;
using System.Collections;

public class AutoDisable : MonoBehaviour {
    private float time = 1;

    void Start() { 
    
    }

    void OnEnable()
    {
        time = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (time <= 0)
        {
            gameObject.SetActive(false);
        }
        else {
            time -= Time.deltaTime;
        }
	}
}
