using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseObj : MonoBehaviour {
    protected int mId;

    public int Id {
        get { return mId; }
    }

    protected string mName;

    public string Name {
        get { return mName; }
        set { mName = value; }
    }

    protected ObjType mObjType;

    public ObjType ObjType {
        get { return mObjType; }
    }

    protected int mUid;

    public int Uid {
        get { return mUid; }
    }

    protected Dictionary<eCmd, OnMsgFunc> mMsgFuncs = new Dictionary<eCmd, OnMsgFunc>();

    protected void RegisterMsgHandler(eCmd cmd,OnMsgFunc func) {
        mMsgFuncs.Add(cmd,func);
    }

    public void OnObjMsg(eCmd cmd, IBaseMsgData baseData) {
        if (mMsgFuncs.ContainsKey(cmd)) {
            mMsgFuncs[cmd](baseData);
        }
    }

    public virtual void Init(int uid) {
        this.mUid = uid;
        this.mObjType = ObjType.None;
        this.mMsgFuncs.Clear();
    }

    public virtual void Enable(Vector3 pos) {
        gameObject.transform.position = pos;
    }

    public virtual void Disable() { }

    private void OnDestroy() {
        OnObjectDestory();
    }

    protected virtual void OnObjectDestory() { }

    protected void Update() {
        this.UpdateObj(Time.deltaTime);
    }

    protected virtual void UpdateObj(float delta) { 
    
    }

    public delegate void OnMsgFunc(IBaseMsgData bmd);
}
