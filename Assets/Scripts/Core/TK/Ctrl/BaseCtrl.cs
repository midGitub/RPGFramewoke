using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseCtrl : MonoBehaviour{
    public delegate void StateFunc(float delta);

    protected bool mAwake;
    public bool Awake {
        get { return mAwake; }
        set { mAwake = value; }
    }
    protected int mId;
    public int Id {
        get { return mId; }
    }
    protected string mName;
    public string Name {
        get { return mName; }
    }
    protected GameObject mOwnerObj;
    protected BaseActor mOwnerScript;
    protected eCmd mCurState;
    protected float mStateTimer;
    protected Dictionary<eState, StateFunc> mStateFuncs = new Dictionary<eState, StateFunc>();

    public virtual void Init(int id,string objectName) {
        mAwake = false;
        mName = objectName + "_Ctrl";
        mOwnerObj=TKObjManager.FindObject(objectName);
        mId = id;
        gameObject.name = mName;
        mStateFuncs.Clear();
        mStateTimer = 0;
    }

    protected void RegisterStateHandler(eState state, StateFunc func)
    {
        if (!mStateFuncs.ContainsKey(state))
            mStateFuncs.Add(state, func);
    }

    protected virtual void UpdateCtrl(float delta) {
        eState state = mOwnerScript.CurState;
        if (mStateFuncs.ContainsKey(state))
            mStateFuncs[state](delta);
    }

    void Update() {
        if (mAwake)
            UpdateCtrl(Time.deltaTime);
    }
}
