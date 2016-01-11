using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseActor : BaseObj{
    protected BaseAICtrl mAICtrl;

    protected BaseCtrl mCtrl;

    protected bool mUpdateShineEff;

    protected int mCurState;

    protected Vector3 mDirection;

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
}
