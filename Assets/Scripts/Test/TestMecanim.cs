using UnityEngine;
using System.Collections;

public class TestMecanim : MonoBehaviour {
    private Obj_Character character1, character2;

	// Use this for initialization
	void Start () {
        character1 = GameObject.Find("ls").GetComponent<Obj_Character>();
        //character2 = GameObject.Find("ls1").GetComponent<Obj_Character>();
	}

    void OnGUI() {
        if (GUI.Button(new Rect(20, 40, 60, 30), "idle"))
        {
            character1.PlayAnima("idle");
        }
        if (GUI.Button(new Rect(80, 40, 60, 30), "run")) {
            character1.PlayAnima("run");
        }
        if (GUI.Button(new Rect(140, 40, 60, 30), "atk1"))
        {
            character1.PlayAnima("atk1");
            //character2.PlayAnima("atk1");
        }
        if (GUI.Button(new Rect(20, 80, 60, 30), "atk2"))
        {
            character1.PlayAnima("atk2");
        }
        if (GUI.Button(new Rect(80, 80, 60, 30), "atk3"))
        {
            character1.PlayAnima("atk3");
        }
        if (GUI.Button(new Rect(140, 80, 60, 30), "atk4"))
        {
            character1.PlayAnima("atk4");
        }
        if (GUI.Button(new Rect(20, 120, 60, 30), "skill1"))
        {
            character1.PlayAnima("skill1");
        }
        if (GUI.Button(new Rect(80, 120, 60, 30), "skill2"))
        {
            character1.PlayAnima("skill2");
        }
        if (GUI.Button(new Rect(140, 120, 60, 30), "roll"))
        {
            character1.PlayAnima("roll");
        }
        if (GUI.Button(new Rect(140, 160, 60, 30), "dead"))
        {
            character1.PlayAnima("dead");
        } 
    }
}
