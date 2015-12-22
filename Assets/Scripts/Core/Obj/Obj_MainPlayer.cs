using UnityEngine;
using System.Collections;

public class Obj_MainPlayer : Obj_Player
{
    public Obj_MainPlayer()
    {
        mObjType = GameGlobalEnum.OBJ_TYPE.OBJ_MAIN_PLAYER;
    }

	// Use this for initialization
	void Start () {
        initBehaviour();
        initNavAgent();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
