using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseHero : BaseCharacter {
    /// <summary>
    /// 账户id
    /// </summary>
    protected int mAccountId;
    public int AccountId {
        set { mAccountId = value; }
    }

    /// <summary>
    /// 战场id
    /// </summary>
    protected int mBattleId;
    public int BattleId {
        set { mBattleId = value; }
    }

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
    public string NickName {
        set { mNickName = value; }
    }
    /// <summary>
    /// 公会名称
    /// </summary>
    protected string mGuildName;
    public string GuildName {
        set { mGuildName = value; }
    }

    /// <summary>
    /// 职业类型
    /// </summary>
    protected JobType mJobType;
    public JobType JobType {
        get { return mJobType; }
    }

    /// <summary>
    /// 英雄技能
    /// </summary>
    private List<HeroSkillInfo>[] mSkillInfos;

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

    public override void Init(int uid)
    {
        base.Init(uid);
        mObjType = ObjType.Min;
        mWeaponCollider=mWeapon.GetComponent<SphereCollider>();
        mBulletWeaponCollider=mBulletWeapom.GetComponent<BoxCollider>();
        mDustEff = mDustObj.GetComponent<ParticleSystem>();

        mSkillInfos = new List<HeroSkillInfo>[2];
        for (int i = 0; i < 2;i++ )
            mSkillInfos[i] = new List<HeroSkillInfo>();
        for (int i = 0; i < mWeaponEffs.Length; i++)
            mWeaponEffs[i] = null;
        mFBX.SendMessage("SetParent",gameObject);
    }

    protected override void UpdateObj(float delta)
    {
        base.UpdateObj(delta);
    }

    public override bool IsPossibleAttacked()
    {
        //急冲，重生
        if (mCurState == 14 || mCurState == 15)
            return false;
        return base.IsPossibleAttacked();
    }

    public virtual bool IsPossibleGetItem() {
        return true;
    }

    public override bool IsPossibleMove()
    {
        //急冲，重生，攻击
        if (mCurState == 14 || mCurState == 15 || mCurState == 16)
            return false;
        return base.IsPossibleMove();
    }

    public virtual bool IsPossibleRush() {
        switch (mCurState) {
            case 3:
            case 8:
            case 9:
            case 4:
            case 5:
            case 16:
                return true;
        }
        return false;
    }

    public override bool IsPossibleUseSkill()
    {
        if (mCurState == 14 || mCurState == 15)
            return false;
        return base.IsPossibleUseSkill();
    }

    public virtual bool IsSkillState() {
        return false;
    }

    public virtual bool IsSkillState(SkillId skillId) {
        return false;
    }

    /// <summary>
    /// 播放受击音效
    /// </summary>
    protected void PlayAttackedMusic() { 
        
    }

    /// <summary>
    /// 播放技能特效
    /// </summary>
    public void OnSkillPlayEff() { 
    
    }
}
