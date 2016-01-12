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

//****************************************************************************************************


    public enum ObjType {
        HERO = 0,
        Max = 2,
        Min = 0,
        Mon = 1,
        None = -1,
    }

    /// <summary>
    /// 基础状态类型
    /// </summary>
    public enum BaseStateType
    {
        ATT = 5,    //攻击
        DEF = 6,    //防御
        HP = 0,     //生命
        MAX = 8,    
        MHP = 3,    //生命上限
        MIN = 0,
        MMP = 4,    //魔法上限    
        MP = 1,     //魔法
        MPC = 2,
        MS = 7,     //移动速度
        NONE = -1

    }

    /// <summary>
    /// 成长类型(等级提升)
    /// </summary>
    public enum GrowthType
    {

        LV = 0,
        Max = 1,
        Min = 0,
        None = -1,
    }

    /// <summary>
    /// 职业类型
    /// </summary>
    public enum JobType {
        NONE = -1,
        ARCHER = 1,      //射手
        WARRIOR = 0,     //战士
        WIZARD = 2,      //魔法师
        ASSASSIN = 3,    //刺客
        MIN = 0,
        MAX = 4,
    }

    public enum OptType
    {
        AC_NA_PIERCER = 0x26,

        ATTR_PVP = 0x1a,
        BLS = 0x18,

        CDRR = 12,

        CRR = 11,
        DEFR = 9,
        DEFR_PVP = 0x1b,
        DOGR = 10,
        DRR = 13,
        FD_DOWNR = 0x23,
        FD_UPR = 0x1f,

        PD_DOWNR = 0x21,
        PD_UPR = 0x1d,

        ID_DOWNR = 0x22,
        ID_UPR = 30,

        TD_DOWNR = 0x24,
        TD_UPR = 0x20,

        PDRR = 15,
        TDRR = 0x12,
        IDRR = 0x10,
        FDRR = 0x11,

        GRC = 0x19,

        HP_PTUP = 0x29,
        HPA = 0x13,
        HPU = 0x15,
        HPU_PVP = 0x1c,

        LUK = 0x17,
        Max = 0x2b,
        Min = 0,
        MP_PTUP = 0x2a,
        MPA = 20,
        MPU = 0x16,
        MSR = 14,
        None = -1,

        PDR = 0,
        IDR = 1,
        FDR = 2,
        TDR = 3,
        ATTR = 4,
        ASR = 5,
        HITR = 6,
        CR = 7,
        CDR = 8,

        TT_DRR = 40,
        WR_NA_STUNR = 0x25,         //战士近身攻击打昏
        WZ_NA_4CHAINR = 0x27
    }

    public enum SkillId {
        Min = 0,
        None = -1,

        //Warrior
        Shock_Wave = 1,     //电击波
        Fatal_Circle = 2,   //致命圈
        ActiveMax = 0x18,
        ActiveMin = 0,
        ATT_UP = 100,
        BLIZZARD = 15,//暴雪
        Rush_Cool_Down = 0x73,
        Whirl_Wind = 4,     //回旋风
        Shadow_Dance = 3,   //影舞
        Freeze_Slash = 0x13,
        Cry_Of_Warrior = 5, //战士哭泣
        Electric_Wind = 20, //电力回旋风

        //Archer
        Multi_Shot = 6,
        Exit_Shot = 8,          //退后射击    
        Arrow_Shower = 7,        //➹鱼
        Flame_Shot = 0x15,      //火箭射击
        Hail_Shower = 0x16,     //冰雹阵
        Phoenix_Bomb = 9,       //凰凰涅盘
        Medusa_Bomb = 0x17,     //美杜莎的炸弹
        Leopard_Tech = 10,      //

        //Assassin
        Rotation_Attack = 110003,//旋转攻击
        Jump_Breaker = 120004,//跳起砸地板
        Blink_Stab = 130005,//背刺

        //Wizard
        Meteorites_Rain = 14,   //陨石雨
        Spirit_Bomb = 11,       //元气弹
        Ice_Wall = 12,          //冰墙
        Thunder_Bolt = 13       //落雷
    }

    public enum BuffType
    {
        AS_UP = 2,
        ATT_UP = 0,
        DEF_UP = 1,
        HEAL = 3,
        HPPORTION = 5,
        MAX = 7,
        MIN = 0,
        MPPORTION = 6,
        None = -1,
        VAMPIRE = 4,
    }

    public enum DeBuffType
    {
        CS = 3,
        FR = 2,         //火球
        IC = 1,
        Max = 4,
        Min = 0,
        None = -1,
        PS = 0,         //中毒

    }

    public enum eCmd
    {
        Idle,           //待机
        Move,
        MoveTo,
        RushMoveTo,
        Turn,
        Attacked,
        AttackedEff,
        Die,            //死亡
        CreateBullet,
        CreateBullets,
        CreateMonsters,
        MonSpawnStart,
        MonSpawnEnd,
        Buff,
        DeBuff,
        LevelUp,        //升级
        Rush,           //急促移动
        RushAttack,     //急冲攻击
        Rebirth,         //重生
        Attack,         //普通攻击
        DashAttack,     //冲撞攻击
        PowerSlash,     //猛砍
        FreezeSlash,    //
        NearAttack,     //近战攻击
        ShootAttack,    //远程攻击
        DropBulletAttack,
        PoisonBall,     //
        WhirlWind,      //旋风斩
        WhirlWindMove,
        ElectricWind,
        ElectricWindMove,
        ShockWave,      //电波流
        ShadowDance,    //影舞
        FatalCircle,    //夺命圈
        //待补充
        //Archer Skill
        MultiShot,
        ArrowShower,
        ArrowShowerTurn,
        ExitShot,
        LeopardTech,
        //Assassin Skill
        RotationAttack,
        JumpBreaker,
        BlinkStab,
        //Wizard Skill
        Meteorites_Rain,
        Spirit_Bomb,
        Ice_Wall,
        Thunder_Bolt,
        //主城动作，移动，静止
        MTIdle,
        MTMove,
        //沙漠Boss技能
        SandStorm,
        MultiPunch,

    }