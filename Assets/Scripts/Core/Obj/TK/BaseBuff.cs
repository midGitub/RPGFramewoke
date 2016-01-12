using UnityEngine;
using System.Collections;

public class BaseBuff{
    protected BaseCharacter mOwner;

    protected BuffType mType;
    public BuffType Type {
        get { return mType; }
    }

    public float mTime;

    protected float mValue;
    public float Value {
        get { return mValue; }
    }

    public virtual void Init(BuffType type,BaseCharacter owner,float time,float value) {
        mType = type;
        mOwner = owner;
        mTime = time;
        mValue = value;
    }

    public static BaseBuff getInstance(BuffType type) {
        return new BaseBuff();
    }

    public virtual void OnDestroy() { }

    public virtual void OnUpdate(float delta) { }
}
