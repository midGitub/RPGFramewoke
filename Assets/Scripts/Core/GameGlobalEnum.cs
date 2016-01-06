using UnityEngine;
using System.Collections;

    public enum OBJ_TYPE
    {
        OBJ,
        OBJ_CHARACTER,
        OBJ_NPC,
        OBJ_OTHER_PLAYER,
        OBJ_MAIN_PLAYER
    }

    public enum OBJ_BEHAVIOUR { 
        NONE,
        IDLE,
        WALK,
        RUN,
        ATK1,
        ATK2,
        ATK3,
        ATK4,
        SKILL1,
        SKILL2,
        HIT,
        ROLL,
        DEAD
    }

    public enum eStaticDataType {
        STATICDATA_SKILL
    }

    public enum eGameState
    {
        Loading = 0,
        Login = 1,
        Home = 2,
    }