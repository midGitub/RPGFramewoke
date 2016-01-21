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
        RegisterState(eState.Sleep, new StateFunc.EnterFunc(SleepEnter),null,null);
        RegisterState(eState.Idle, new StateFunc.EnterFunc(IdleEnter), null, new StateFunc.ExitFunc(IdleExit));
        RegisterState(eState.Move, new StateFunc.EnterFunc(MoveEnter), new StateFunc.UpdateFunc(MoveUpdate), new StateFunc.ExitFunc(MoveEixt));
        //todo Register other state

        RegisterMsgHandler(eCmd.Idle, new OnMsgFunc(OnMsgIdle));
        RegisterMsgHandler(eCmd.Rush, new OnMsgFunc(OnMsgRush));
        RegisterMsgHandler(eCmd.Move, new OnMsgFunc(OnMsgMove));
        RegisterMsgHandler(eCmd.Attack, new OnMsgFunc(OnMsgAttack));
        //todo Register other msg
    }


    #region StateFunction
    private void SleepEnter(StateParam param)
    { 
    
    }

    private void IdleEnter(StateParam param) { 
    
    }

    private void IdleExit() { 
    
    }

    private void MoveEnter(StateParam param) { 
    
    }

    private void MoveUpdate(float delta) { 
    
    }

    private void MoveEixt() { 
        
    }
    #endregion

    #region MsgCallback
    private void OnMsgIdle(IBaseMsgData bmd)
    {
        if (!IsDead()) {
            ChangeState(eState.Idle, null);
        }
    }

    private void OnMsgRush(IBaseMsgData bmd)
    {
        if (!IsDead())
        {
            ChangeState(eState.Rush, null);
        }
    }

    private void OnMsgMove(IBaseMsgData bmd)
    {
        if (!IsDead())
        {
            ChangeState(eState.Move, null);
        }
    }

    private void OnMsgAttack(IBaseMsgData bmd)
    {
        if (!IsDead())
        {
            ChangeState(eState.Attack, null);
        }
    }
    #endregion

    #region AnimationCallback
    public void OnAttackEnd() {
        if (!IsDead())
            ChangeState(eState.Idle,null);
    }

    public void OnSkillEnd() {
        if (!IsDead())
            ChangeState(eState.Idle,null);
    }
    #endregion

    protected override void UpdateObj(float delta)
    {
        base.UpdateObj(delta);
    }

    public override bool IsPossibleAttacked()
    {
        //急冲，重生
        if (mCurState == eState.Rush || mCurState == eState.Rebirth)
            return false;
        return base.IsPossibleAttacked();
    }

    public virtual bool IsPossibleGetItem() {
        return true;
    }

    public override bool IsPossibleMove()
    {
        //急冲，重生，攻击
        if (mCurState == eState.Rush || mCurState == eState.Rebirth || mCurState == eState.Attack)
            return false;
        return base.IsPossibleMove();
    }

    public virtual bool IsPossibleRush() {
        switch (mCurState) {
            case eState.Idle:
            case eState.Move:
            case eState.MoveTo:
            case eState.Stiff:
            case eState.Slide:
            case eState.Attack:
                return true;
        }
        return false;
    }

    public override bool IsPossibleUseSkill()
    {
        if (mCurState == eState.Rush || mCurState == eState.Rebirth)
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
