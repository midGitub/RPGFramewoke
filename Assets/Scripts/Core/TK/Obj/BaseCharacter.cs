using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseCharacter : BaseActor {
    /// <summary>
    /// 攻击特效播放点
    /// </summary>
    public GameObject mEffPoint;

    /// <summary>
    /// 特效持续最长时间
    /// </summary>
    protected float mAttackEffMaxTime;

    protected bool bCameraFrustum;
    public bool CameraFrustum {
        get { return bCameraFrustum; }
    }

    /// <summary>
    /// 碰撞后是否移动
    /// </summary>
    protected bool bCollisionMove;
    public bool CollisionMove {
        set { bCollisionMove = value; }
    }

    /// <summary>
    /// 是否静音
    /// </summary>
    protected bool bMute;

    /// <summary>
    /// 子弹数量
    /// </summary>
    protected int mBulletCount;
    public int BulletCount {
        get { return mBulletCount++; }
    }

    /// <summary>
    /// 胶囊碰撞
    /// </summary>
    protected CapsuleCollider mCapsuleCollider;

    /// <summary>
    /// 移动朝向
    /// </summary>
    protected Vector3 mMoveDirection;

    /// <summary>
    /// 移动目标点
    /// </summary>
    protected Vector3 mMovePosition;

    /// <summary>
    /// 屏幕坐标点
    /// </summary>
    protected Vector3 mScreenPosition;
    public Vector3 ScreenPosition {
        get { return mScreenPosition; }
    }

    /// <summary>
    /// 打晕特效
    /// </summary>
    protected GameObject mStunEff;

    /// <summary>
    /// 打晕最长持续时间
    /// </summary>
    protected float mStunMaxTime;

    /// <summary>
    /// 队伍id
    /// </summary>
    protected int mTeamId;
    public int TeamId {
        get { return mTeamId; }
        set { mTeamId = value; }
    }
    
    /// <summary>
    /// 角色状态
    /// </summary>
    protected CharacterState mState = new CharacterState();

    /// <summary>
    /// 所有增益buff
    /// </summary>
    protected List<BaseBuff> mBuffs = new List<BaseBuff>();

    /// <summary>
    /// 所有负面buff
    /// </summary>
    protected List<BaseDeBuff> mDeBuffs = new List<BaseDeBuff>();

    public override void Init(int uid)
    {
        base.Init(uid);
        bMute = false;
        mFBX.SendMessage("SetParent",gameObject);
        mCapsuleCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    public override void Enable(Vector3 pos)
    {
        base.Enable(pos);
        bCameraFrustum = false;
        mAttackEffMaxTime = 0;
        mMoveDirection = Vector3.zero;
        mBulletCount = 0;
        mStunMaxTime = 0;
        mBuffs.Clear();
        mDeBuffs.Clear();
        bCollisionMove = true;
    }

    protected override void UpdateObj(float delta)
    {
        base.UpdateObj(delta);
        UpdateBuff(delta);
        UpdateDeBuff(delta);

    }

    protected virtual void UpdateBuff(float delta) { 
        foreach(BaseBuff bBuff in mBuffs){
            bBuff.OnUpdate(delta);
        }
        for (int i = mBuffs.Count - 1; i >= 0; i--) {
            mBuffs[i].mTime -= delta;
            if (mBuffs[i].mTime < 0) {
                mBuffs[i].OnDestroy();
                mBuffs.RemoveAt(i);
            }
        }
    }

    protected virtual void UpdateDeBuff(float delta) {
        foreach (BaseDeBuff bDeBuff in mDeBuffs)
        {
            bDeBuff.OnUpdate(delta);
        }
        for (int i = mDeBuffs.Count - 1; i >= 0; i--)
        {
            mDeBuffs[i].mTime -= delta;
            if (mDeBuffs[i].mTime < 0)
            {
                mDeBuffs[i].OnDestroy();
                mDeBuffs.RemoveAt(i);
            }
        }
    }

    protected void AddBuff(BuffType type,float value,float time) {
        BaseBuff buff=mBuffs.Find(x=>x.Type==type);
        if (buff != null)
            buff.Init(type, this, time, value);
        else {
            BaseBuff instance= BaseBuff.getInstance(type);
            instance.Init(type, this, time, value);
            mBuffs.Add(instance);
        }

    }

    protected void AddDeBuff(DeBuffType type,float value,float time) {
        BaseDeBuff deBuff=mDeBuffs.Find(x => x.Type == type);
        if (deBuff != null)
            deBuff.Init(type, this, time, value);
        else {
            BaseDeBuff instance=BaseDeBuff.getInstance(type);
            instance.Init(type,this,time,value);
            mDeBuffs.Add(instance);
        }
    }

    protected float GetBuffValue(BuffType type) {
        BaseBuff buff = mBuffs.Find(x => x.Type == type);
        if (buff != null)
            return buff.Value;
        return 0;
    }

    protected float GetDeBuffValue(DeBuffType type) {
        BaseDeBuff deBuff=mDeBuffs.Find(x=>x.Type==type);
        if (deBuff != null)
            return deBuff.Value;
        return 0;
    }

    protected override void OnObjectDestory()
    {
        base.OnObjectDestory();
        mStunEff = null;
    }

    /// <summary>
    /// 是否可以被攻击
    /// </summary>
    public virtual bool IsPossibleAttacked() {
        if (mCurState == eState.Sleep || mCurState == eState.Die)
            return false;
        return true;
    }

    /// <summary>
    /// 是否可以移动
    /// </summary>
    public virtual bool IsPossibleMove() {
        switch (mCurState)
        {
            case eState.Sleep:
            case eState.Die:    //死亡
            case eState.Slide:     //滑动 
            case eState.Down:    //打倒
            case eState.Stun:    //打昏
            case eState.CriticalStiff:
                return false;
        }
        return true;
    }

    /// <summary>
    /// 是否可以使用技能
    /// </summary>
    public virtual bool IsPossibleUseSkill() {
        return IsPossibleMove();
    }

    protected bool CheckGrowthType(GrowthType type) {
        if (type >= GrowthType.Min && type < GrowthType.Max)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 设置成长属性
    /// </summary>
    public void SetGrowth(GrowthType type,float value) {
        
    }

    /// <summary>
    /// 获取成长
    /// </summary>
    public void GetGrowth(GrowthType type) { 
    
    }

    protected bool CheckStateBase(BaseStateType stateType) {
        if (stateType >= BaseStateType.MIN && stateType < BaseStateType.MAX)
            return true;
        else
            return false;
    }

    public void SetStateBase(BaseStateType stateType,float value) { 
        
    }

    public void GetStateBase(BaseStateType stateType) { 
    
    }

    protected bool CheckStateOption(OptType type) {
        if (type >= OptType.Min && type < OptType.Max)
            return true;
        else
            return false;
    }

    public void SetStateOption(OptType type, float value) { 
    
    }

    public void GetStateOption(OptType type) { 
    
    }

    /// <summary>
    /// 是否有负面状态
    /// </summary>
    protected bool HasDebuff(DeBuffType type) {
       return mDeBuffs.Find(x => x.Type == type) != null;
    }

    /// <summary>
    /// 判断是否死亡
    /// </summary>
    public bool IsDead() {
        return mCurState == eState.Die;
    }

    /// <summary>
    /// 播放受击特效
    /// </summary>
    public virtual void PlayAttackedEff() { 
        
    }

    public virtual void SelfDamage() { 
    
    }
}
public enum eState
{
    None,//0
    Sleep,
    Advent,
    Idle,
    Move,
    MoveTo,//5
    Turn,
    CriticalStiff,  //最后一击
    Stiff,
    Slide,          //滑动
    Down,//10
    StunSlide,
    Stun,
    Die,
    Rush,//14
    Rebirth,
    Attack
}
