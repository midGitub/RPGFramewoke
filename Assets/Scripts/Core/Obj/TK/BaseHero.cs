using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseHero : BaseCharacter {
    /// <summary>
    /// 账户id
    /// </summary>
    protected int mAccountId;
    /// <summary>
    /// 战场id
    /// </summary>
    protected int mBattleId;
    /// <summary>
    /// 自动攻击范围
    /// </summary>
    protected float mAutoAttackRange;

    public GameObject mBulletWeapom;

    protected BoxCollider mBulletWeaponCollider;

    protected SphereCollider mWeaponCollider;

    protected string mCurAttackAnimName;

    protected float mCurAttackAniSpeed;

    protected int mCurAttackIndex;

    protected SkillId mCurSkillId;

    public float mCurSkillRange;

    protected float mCurSkillTimeMax;

    /// <summary>
    /// 走路尘土
    /// </summary>
    public GameObject mDustObj;
    protected ParticleSystem mDustEff;

    /// <summary>
    /// 昵称
    /// </summary>
    protected string mNickName;
    /// <summary>
    /// 公会名称
    /// </summary>
    protected string mGuildName;

    /// <summary>
    /// 职业类型
    /// </summary>
    protected JobType mJobType;

    /// <summary>
    /// 英雄技能
    /// </summary>
    private List<HeroSkillInfo> mSkillInfos;

    /// <summary>
    /// 拖尾特效
    /// </summary>
    public GameObject[] mTrailEffs=new GameObject[2];

    /// <summary>
    /// 武器
    /// </summary>
    public GameObject mWeapon;

    /// <summary>
    /// 武器特效
    /// </summary>
    protected GameObject[] mWeaponEffs=new GameObject[2];

    public float mMoveSpeed = 5;

    public float mRushSpeed = 8;

    public float mRushTime = 0.5f;

}
