using UnityEngine;
using System.Collections;

public class Obj_MainPlayer : Obj_Player
{
    public Obj_MainPlayer()
    {
        mObjType = OBJ_TYPE.OBJ_MAIN_PLAYER;
    }

	// Use this for initialization
	void Start () {
        initNavAgent();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
