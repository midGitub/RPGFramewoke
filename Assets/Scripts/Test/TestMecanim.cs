using UnityEngine;
using System.Collections;

public class TestMecanim : MonoBehaviour {
    private Obj_Character character;

	// Use this for initialization
	void Start () {
	    character=GetComponent<Obj_Character>();
	}

    void OnGUI() {
        if (GUI.Button(new Rect(20, 40, 60, 30), "idle"))
        {
            character.Idle();
        }
        if (GUI.Button(new Rect(80, 40, 60, 30), "run")) {
            character.Run();
        }
        if (GUI.Button(new Rect(140, 40, 60, 30), "atk1"))
        {
            character.Atk1();
        }
        if (GUI.Button(new Rect(20, 80, 60, 30), "atk2"))
        {
            character.Atk2();
        }
        if (GUI.Button(new Rect(80, 80, 60, 30), "atk3"))
        {
            character.Atk3();
        }
        if (GUI.Button(new Rect(140, 80, 60, 30), "atk4"))
        {
            character.Atk4();
        }
        if (GUI.Button(new Rect(20, 120, 60, 30), "skill1"))
        {
            character.Skill1();
        }
        if (GUI.Button(new Rect(80, 120, 60, 30), "skill2"))
        {
            character.Skill2();
        }
        if (GUI.Button(new Rect(140, 120, 60, 30), "roll"))
        {
            character.Roll();
        }
        if (GUI.Button(new Rect(140, 160, 60, 30), "dead"))
        {
            character.Dead();
        } 
    }
}
