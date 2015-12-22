using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjManager : SingletonObject<ObjManager> {
    private Obj_MainPlayer mainPlayer;
    private Dictionary<int, Obj> objPools = new Dictionary<int, Obj>();

    public Obj_MainPlayer MainPlayer {
        get { return mainPlayer; }
    }


    public void CreateMainPlayer() {
        
    }

    private void CreateOtherPlayer() { 
        
    }

    private void CreateNpc()
    { 
        
    }

    public void CreateCharacter(GameGlobalEnum.OBJ_TYPE objType) { 
        switch(objType){
            case GameGlobalEnum.OBJ_TYPE.OBJ_NPC:
                CreateNpc();
                break;
            case GameGlobalEnum.OBJ_TYPE.OBJ_OTHER_PLAYER:
                CreateOtherPlayer();
                break;
        }
    }
}
