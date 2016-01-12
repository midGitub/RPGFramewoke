using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BaseActor : BaseObj{
    protected BaseAICtrl mAICtrl;

    public BaseAICtrl AICtrl {
        get { return mAICtrl; }
        set { mAICtrl = value; }
    }

    protected BaseCtrl mCtrl;

    public BaseCtrl Ctrl {
        get { return mCtrl; }
        set { mCtrl = value; }
    }

    protected bool mUpdateShineEff;

    protected int mCurState;

    public int CurState {
        get { return mCurState; }
    }

    protected Vector3 mDirection;

    public Vector3 Direction {
        get { return mDirection; }
    }

    protected Animator mAnimator;

    protected GameObject mFBX;

    protected List<Material> mMaterials = new List<Material>();

    protected int mPreState;

    public GameObject mShadow;

    protected float mShineTimeMax;

    protected float mShineTimer;

    protected float mShineValue;

    protected Dictionary<int, StateFunc> mStateFunc = new Dictionary<int, StateFunc>();

    protected StateParam mStateParam = new StateParam();

    protected float mStateTimer;

    public override void Init(int uid)
    {
        base.Init(uid);
        mStateFunc.Clear();
        RefreshMaterials();
        if (mFBX != null)
            mAnimator = mFBX.GetComponent<Animator>();
    }

    public override void Enable(Vector3 pos)
    {
        base.Enable(pos);
        mPreState = 0;
        mCurState = 0;
        mStateTimer = 0;
        mDirection = Vector3.forward;
        mUpdateShineEff = false;
        mShineTimer = 0;
        mShineValue = 0;

    }

    protected override void UpdateObj(float delta)
    {
        base.UpdateObj(delta);
        if (mUpdateShineEff)
            UpdateShineEff(delta);
        if (mStateFunc.ContainsKey(mCurState) && mStateFunc[mCurState].updateFunc!=null)
            mStateFunc[mCurState].updateFunc(delta);
    }

    public void RegisterState(int state,StateFunc.EnterFunc enFunc,StateFunc.UpdateFunc updateFunc,StateFunc.ExitFunc exitFunc) {
        if (!mStateFunc.ContainsKey(state)) {
            StateFunc func = new StateFunc { 
                enterFunc=enFunc,
                updateFunc=updateFunc,
                exitFunc=exitFunc
            };
            mStateFunc.Add(state,func);
        }
    }

    public void RegisterEvent(string aniName, string funcName, float time)
    {
        AnimationEvent evt = new AnimationEvent
        {
            time = time,
            functionName = funcName,
            intParameter = mUid
        };
        AnimationClip ac = this.mFBX.GetComponent<Animation>().GetClip(aniName);
        if (ac == null)
            Debug.LogError("Not Found the AnimationClip:" + aniName);
        else
            ac.AddEvent(evt);
    }

    public void ChangeState(int state,StateParam param) {
        if (mStateFunc.ContainsKey(mCurState) && mStateFunc[mCurState].exitFunc!=null)
            mStateFunc[mCurState].exitFunc();
        mPreState = mCurState;
        mCurState = state;
        if (mStateFunc.ContainsKey(state) && mStateFunc[state].enterFunc != null)
            mStateFunc[state].enterFunc(param);
        mStateTimer = 0;
    }

    public void ChangeAnimSpeed(string aniName,float speed) {
        Animation animation=mFBX.GetComponent<Animation>();
        if (animation != null) {
            animation[aniName].speed = speed;
        }
    }

    public void Look(Vector3 dir) {
        if (dir != Vector3.zero) {
            dir.Normalize();
            base.gameObject.transform.localRotation = Quaternion.Euler(dir);
            mDirection = dir;
        }
    }

    public void TurnAngle(float angle,float duration) { 
    
    }

    public void TurnTarget(Vector3 target,float duration) { 
    
    }

    public void PlayAnim(string condition,string value=null,Type t=null,float speed=1) {
        mAnimator.speed = speed;
        if (typeof(int) == t)
            mAnimator.SetInteger(condition, int.Parse(value));
        else if (typeof(float) == t)
            mAnimator.SetFloat(condition, float.Parse(value));
        else if (typeof(bool) == t)
            mAnimator.SetBool(condition, bool.Parse(value));
        else
            mAnimator.SetTrigger(value);
    }

    public void RefreshMaterials() {
        mMaterials.Clear();
        SkinnedMeshRenderer[] skinnedRenders= gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < skinnedRenders.Length; i++)
        {
            for (int j = 0; j < skinnedRenders[i].materials.Length;j++ )
            {
                mMaterials.Add(skinnedRenders[i].materials[j]);
            }
        }
        MeshRenderer[] meshRenders= gameObject.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenders.Length; i++)
        {
            for (int j = 0; j < meshRenders[i].materials.Length; j++)
            {
                mMaterials.Add(meshRenders[i].materials[j]);
            }
        }
    }

    public void RefreshShineEff(Color color) {
        foreach (Material m in mMaterials)
            m.SetColor("_AddColor", color);
    }

    public void RefreshShineEff(float shineValue) {
        foreach (Material m in mMaterials)
            m.SetFloat("_ShineValue", shineValue);
    }

    public void UpdateShineEff(float delta) {
        mShineTimer += delta;
        if (mShineTimer > mShineTimeMax)
        {
            mUpdateShineEff = false;
            mShineTimer = 0;
            mShineValue = 0;
            RefreshShineEff(new Color(0, 0, 0, 1));
        }
        else {
            float percent = mShineValue / mShineTimeMax;
            mShineValue = 1 - percent;
            mShineValue *= 5;
            RefreshShineEff(new Color(1, 1, 1, 1));
        }
    }

    public void ShineEff(float delay=0.15f) {
        mUpdateShineEff = true;
        mShineTimer = 0;
        mShineTimeMax = delay;
    }
}
