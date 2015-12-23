using UnityEngine;
using System.Collections;

public class Obj_Player : Obj_Character
{
    private int profession;

    public int Profession {
        set { profession = value; }
        get { return profession; }
    }

    public Obj_Player()
    {
        mObjType = OBJ_TYPE.OBJ_OTHER_PLAYER;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
